#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using UnityEngine.InputSystem.Utilities;
using UnityEditor;

////TODO: unify the generated events so that performed, canceled, and started all go into a single event

////TODO: look up actions and maps by ID rather than by name

////TODO: only generate @something if @ is really needed

////TODO: allow having an unnamed or default-named action set which spills actions directly into the toplevel wrapper

////TODO: add cleanup for ActionEvents

////TODO: protect generated wrapper against modifications made to asset

////TODO: make capitalization consistent in the generated code

////REVIEW: allow putting *all* of the data from the inputactions asset into the generated class?

namespace UnityEngine.InputSystem.Editor
{
    /// <summary>
    /// Utility to generate code that makes it easier to work with action sets.
    /// </summary>
    public static class InputActionCodeGenerator
    {
        private const int kSpacesPerIndentLevel = 4;

        public struct Options
        {
            public string className { get; set; }
            public string namespaceName { get; set; }
            public string sourceAssetPath { get; set; }
        }

        public static string GenerateWrapperCode(InputActionAsset asset, Options options = default)
        {
            if (asset == null)
                throw new ArgumentNullException(nameof(asset));

            if (string.IsNullOrEmpty(options.sourceAssetPath))
                options.sourceAssetPath = AssetDatabase.GetAssetPath(asset);
            if (string.IsNullOrEmpty(options.className) && !string.IsNullOrEmpty(asset.name))
                options.className =
                    CSharpCodeHelpers.MakeTypeName(asset.name);

            if (string.IsNullOrEmpty(options.className))
            {
                if (string.IsNullOrEmpty(options.sourceAssetPath))
                    throw new ArgumentException("options.sourceAssetPath");
                options.className =
                    CSharpCodeHelpers.MakeTypeName(Path.GetFileNameWithoutExtension(options.sourceAssetPath));
            }

            var writer = new Writer
            {
                buffer = new StringBuilder()
            };

            // Header.
            if (!string.IsNullOrEmpty(options.sourceAssetPath))
                writer.WriteLine($"// GENERATED AUTOMATICALLY FROM '{options.sourceAssetPath}'\n");

            // Usings.
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using UnityEngine.InputSystem;");
            writer.WriteLine("using UnityEngine.InputSystem.Utilities;");
            writer.WriteLine("");

            // Begin namespace.
            var haveNamespace = !string.IsNullOrEmpty(options.namespaceName);
            if (haveNamespace)
            {
                writer.WriteLine($"namespace {options.namespaceName}");
                writer.BeginBlock();
            }

            // Begin class.
            writer.WriteLine($"public class @{options.className} : IInputActionCollection, IDisposable");
            writer.BeginBlock();

            writer.WriteLine($"private InputActionAsset asset;");

            // Default constructor.
            writer.WriteLine($"public @{options.className}()");
            writer.BeginBlock();
            writer.WriteLine($"asset = InputActionAsset.FromJson(@\"{asset.ToJson().Replace("\"", "\"\"")}\");");

            var maps = asset.actionMaps;
            var schemes = asset.controlSchemes;
            foreach (var map in maps)
            {
                var mapName = CSharpCodeHelpers.MakeIdentifier(map.name);
                writer.WriteLine($"// {map.name}");
                writer.WriteLine($"m_{mapName} = asset.FindActionMap(\"{map.name}\", throwIfNotFound: true);");

                foreach (var action in map.actions)
                {
                    var actionName = CSharpCodeHelpers.MakeIdentifier(action.name);
                    writer.WriteLine($"m_{mapName}_{actionName} = m_{mapName}.FindAction(\"{action.name}\", throwIfNotFound: true);");
                }
            }
            writer.EndBlock();
            writer.WriteLine();

            writer.WriteLine("public void Dispose()");
            writer.BeginBlock();
            writer.WriteLine("UnityEngine.Object.Destroy(asset);");
            writer.EndBlock();
            writer.WriteLine();

            writer.WriteLine("public InputBinding? bindingMask");
            writer.BeginBlock();
            writer.WriteLine("get => asset.bindingMask;");
            writer.WriteLine("set => asset.bindingMask = value;");
            writer.EndBlock();
            writer.WriteLine();

            writer.WriteLine("public ReadOnlyArray<InputDevice>? devices");
            writer.BeginBlock();
            writer.WriteLine("get => asset.devices;");
            writer.WriteLine("set => asset.devices = value;");
            writer.EndBlock();
            writer.WriteLine();

            writer.WriteLine("public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;");
            writer.WriteLine();

            writer.WriteLine("public bool Contains(InputAction action)");
            writer.BeginBlock();
            writer.WriteLine("return asset.Contains(action);");
            writer.EndBlock();
            writer.WriteLine();

            writer.WriteLine("public IEnumerator<InputAction> GetEnumerator()");
            writer.BeginBlock();
            writer.WriteLine("return asset.GetEnumerator();");
            writer.EndBlock();
            writer.WriteLine();

            writer.WriteLine("IEnumerator IEnumerable.GetEnumerator()");
            writer.BeginBlock();
            writer.WriteLine("return GetEnumerator();");
            writer.EndBlock();
            writer.WriteLine();

            writer.WriteLine("public void Enable()");
            writer.BeginBlock();
            writer.WriteLine("asset.Enable();");
            writer.EndBlock();
            writer.WriteLine();

            writer.WriteLine("public void Disable()");
            writer.BeginBlock();
            writer.WriteLine("asset.Disable();");
            writer.EndBlock();

            // Action map accessors.
            foreach (var map in maps)
            {
                writer.WriteLine();
                writer.WriteLine($"// {map.name}");

                var mapName = CSharpCodeHelpers.MakeIdentifier(map.name);
                var mapTypeName = CSharpCodeHelpers.MakeTypeName(mapName, "Actions");

                // Caching field for action map.
                writer.WriteLine($"private readonly InputActionMap m_{mapName};");
                writer.WriteLine(string.Format("private I{0} m_{0}CallbackInterface;", mapTypeName));

                // Caching fields for all actions.
                foreach (var action in map.actions)
                {
                    var actionName = CSharpCodeHelpers.MakeIdentifier(action.name);
                    writer.WriteLine($"private readonly InputAction m_{mapName}_{actionName};");
                }

                // Struct wrapping access to action set.
                writer.WriteLine($"public struct {mapTypeName}");
                writer.BeginBlock();

                // Constructor.
                writer.WriteLine($"private @{options.className} m_Wrapper;");
                writer.WriteLine($"public {mapTypeName}(@{options.className} wrapper) {{ m_Wrapper = wrapper; }}");

                // Getter for each action.
                foreach (var action in map.actions)
                {
                    var actionName = CSharpCodeHelpers.MakeIdentifier(action.name);
                    writer.WriteLine(
                        $"public InputAction @{actionName} => m_Wrapper.m_{mapName}_{actionName};");
                }

                // Action map getter.
                writer.WriteLine($"public InputActionMap Get() {{ return m_Wrapper.m_{mapName}; }}");

                // Enable/disable methods.
                writer.WriteLine("public void Enable() { Get().Enable(); }");
                writer.WriteLine("public void Disable() { Get().Disable(); }");
                writer.WriteLine("public bool enabled => Get().enabled;");

                // Implicit conversion operator.
                writer.WriteLine(
                    $"public static implicit operator InputActionMap({mapTypeName} set) {{ return set.Get(); }}");

                // SetCallbacks method.
                writer.WriteLine($"public void SetCallbacks(I{mapTypeName} instance)");
                writer.BeginBlock();

                ////REVIEW: this would benefit from having a single callback on InputActions rather than three different endpoints

                // Uninitialize existing interface.
                writer.WriteLine($"if (m_Wrapper.m_{mapTypeName}CallbackInterface != null)");
                writer.BeginBlock();
                foreach (var action in map.actions)
                {
                    var actionName = CSharpCodeHelpers.MakeIdentifier(action.name);
                    var actionTypeName = CSharpCodeHelpers.MakeTypeName(action.name);

                    writer.WriteLine($"@{actionName}.started -= m_Wrapper.m_{mapTypeName}CallbackInterface.On{actionTypeName};");
                    writer.WriteLine($"@{actionName}.performed -= m_Wrapper.m_{mapTypeName}CallbackInterface.On{actionTypeName};");
                    writer.WriteLine($"@{actionName}.canceled -= m_Wrapper.m_{mapTypeName}CallbackInterface.On{actionTypeName};");
                }
                writer.EndBlock();

                // Initialize new interface.
                writer.WriteLine($"m_Wrapper.m_{mapTypeName}CallbackInterface = instance;");
                writer.WriteLine("if (instance != null)");
                writer.BeginBlock();
                foreach (var action in map.actions)
                {
                    var actionName = CSharpCodeHelpers.MakeIdentifier(action.name);
                    var actionTypeName = CSharpCodeHelpers.MakeTypeName(action.name);

                    writer.WriteLine($"@{actionName}.started += instance.On{actionTypeName};");
                    writer.WriteLine($"@{actionName}.performed += instance.On{actionTypeName};");
                    writer.WriteLine($"@{actionName}.canceled += instance.On{actionTypeName};");
                }
                writer.EndBlock();
                writer.EndBlock();
                writer.EndBlock();

                // Getter for instance of struct.
                writer.WriteLine($"public {mapTypeName} @{mapName} => new {mapTypeName}(this);");
            }

            // Control scheme accessors.
            foreach (var scheme in schemes)
            {
                var identifier = CSharpCodeHelpers.MakeIdentifier(scheme.name);

                writer.WriteLine($"private int m_{identifier}SchemeIndex = -1;");
                writer.WriteLine($"public InputControlScheme {identifier}Scheme");
                writer.BeginBlock();
                writer.WriteLine("get");
                writer.BeginBlock();
                writer.WriteLine($"if (m_{identifier}SchemeIndex == -1) m_{identifier}SchemeIndex = asset.FindControlSchemeIndex(\"{scheme.name}\");");
                writer.WriteLine($"return asset.controlSchemes[m_{identifier}SchemeIndex];");
                writer.EndBlock();
                writer.EndBlock();
            }

            // Generate interfaces.
            foreach (var map in maps)
            {
                var typeName = CSharpCodeHelpers.MakeTypeName(map.name);
                writer.WriteLine($"public interface I{typeName}Actions");
                writer.BeginBlock();

                foreach (var action in map.actions)
                {
                    var methodName = CSharpCodeHelpers.MakeTypeName(action.name);
                    writer.WriteLine($"void On{methodName}(InputAction.CallbackContext context);");
                }

                writer.EndBlock();
            }

            // End class.
            writer.EndBlock();

            // End namespace.
            if (haveNamespace)
                writer.EndBlock();

            return writer.buffer.ToString();
        }

        private struct Writer
        {
            public StringBuilder buffer;
            public int indentLevel;

            public void BeginBlock()
            {
                WriteIndent();
                buffer.Append("{\n");
                ++indentLevel;
            }

            public void EndBlock()
            {
                --indentLevel;
                WriteIndent();
                buffer.Append("}\n");
            }

            public void WriteLine()
            {
                buffer.Append('\n');
            }

            public void WriteLine(string text)
            {
                WriteIndent();
                buffer.Append(text);
                buffer.Append('\n');
            }

            public void Write(string text)
            {
                buffer.Append(text);
            }

            public void WriteIndent()
            {
                for (var i = 0; i < indentLevel; ++i)
                {
                    for (var n = 0; n < kSpacesPerIndentLevel; ++n)
                        buffer.Append(' ');
                }
            }
        }

        // Updates the given file with wrapper code generated for the given action sets.
        // If the generated code is unchanged, does not touch the file.
        // Returns true if the file was touched, false otherwise.
        public static bool GenerateWrapperCode(string filePath, InputActionAsset asset, Options options)
        {
            if (!Path.HasExtension(filePath))
                filePath += ".cs";

            // Generate code.
            var code = GenerateWrapperCode(asset, options);

            // Check if the code changed. Don't write if it hasn't.
            if (File.Exists(filePath))
            {
                var existingCode = File.ReadAllText(filePath);
                if (existingCode == code || existingCode.WithAllWhitespaceStripped() == code.WithAllWhitespaceStripped())
                    return false;
            }

            // Write.
            File.WriteAllText(filePath, code);
            return true;
        }
    }
}
#endif // UNITY_EDITOR
