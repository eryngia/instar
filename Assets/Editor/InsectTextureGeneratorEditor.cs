using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TextureGeneratorEditor : EditorWindow {
    private int pixelSize = 32;
    private int textureCount = 12; 
    private int columns = 3; 
    private int highlightCount = 0;
    private Texture2D[] generatedTextures;
    private Texture2D[] alternateTextures;  // Store alternate set

    private const float previewScale = 1f;
    private bool showFirstSet = true; // Toggle state

    [MenuItem("Tools/Texture Generator")]
    public static void ShowWindow() {
        GetWindow<TextureGeneratorEditor>("Texture Generator");
    }

    private void OnGUI() {
        GUILayout.Label("Texture Generator", EditorStyles.boldLabel);

        pixelSize = EditorGUILayout.IntField("Pixel Size", pixelSize);
        highlightCount = EditorGUILayout.IntField("Highlights", highlightCount);
        textureCount = EditorGUILayout.IntField("Texture Count", textureCount);
        columns = EditorGUILayout.IntField("Grid Columns", columns);

        if (GUILayout.Button("Generate Textures")) {
            GenerateTextures();
        }

        if (generatedTextures != null && alternateTextures != null && generatedTextures.Length > 0) {
            GUILayout.Label("Generated Textures:");

            // Button to switch all textures
            if (GUILayout.Button($"Switch Animation (Showing: {(showFirstSet ? "Anim 1" : "Anim 2")})")) {
                showFirstSet = !showFirstSet;
            }

            int rows = Mathf.CeilToInt((float)textureCount / columns);
            float previewSize = 100f * previewScale;

            for (int row = 0; row < rows; row++) {
                GUILayout.BeginHorizontal();
                for (int col = 0; col < columns; col++) {
                    int index = row * columns + col;

                    if (index < textureCount) {
                        // Switch between the two sets
                        Texture2D textureToShow = showFirstSet ? generatedTextures[index] : alternateTextures[index];

                        if (textureToShow != null) {
                            Rect textureRect = GUILayoutUtility.GetRect(previewSize, previewSize, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
                            EditorGUI.DrawPreviewTexture(textureRect, textureToShow);
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
    }

    private void GenerateTextures() {
        generatedTextures = new Texture2D[textureCount];
        alternateTextures = new Texture2D[textureCount];

        for (int i = 0; i < textureCount; i++) {
            Texture2D[] textures = InsectTextureGenerator.Generate(pixelSize, highlightCount).textures;

            generatedTextures[i] = textures[0];
            alternateTextures[i] = textures[1]; 
        }
    }
}
