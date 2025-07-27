using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Xml;
using UnityEngine.UIElements;

namespace EditorNiveles
{
    public class PaletteWindow : EditorWindow
    {
        public static PaletteWindow instance;

        // TODO: agrupar los items en categorias. En el editor se crearan tantas 
        // carpetas como categorias de items haya y se guardara cada item en su 
        // carpeta correspondiente. Maybe agruparlos aqui con un diccionario de listas??

        private Dictionary<string, List<GameObject>> categories;
        private List<string> categoryLabels;
        private string selectedCategory;
        private string lastCategory;

        private List<GameObject> items;
        private Dictionary<string, Dictionary<string, Texture2D>> previews;
        private Vector2 scrollPosition;
        private const float buttonWidth = 80;
        private const float buttonHeight = 90;
        private Texture2D defaultPreview;

        public delegate void itemSelectedDelegate(GameObject item);
        public static event itemSelectedDelegate ItemSelectedEvent;

        public static void ShowPalette()
        {
            instance = (PaletteWindow)GetWindow(typeof(PaletteWindow));
            instance.titleContent = new GUIContent("Palette");
        }

        private void OnEnable()
        {
            InitializeCategories();
            LoadTexture();
            MetaDataManager.metadataChangedEvent += InitializeCategories;
        }

        private void OnDisable()
        {
            MetaDataManager.metadataChangedEvent -= InitializeCategories;
        }

        private void Update()
        {
            if (selectedCategory != lastCategory)
            {
                categories[selectedCategory] = MetaDataManager.GetCategory(selectedCategory);
                items = categories[selectedCategory];
                lastCategory = selectedCategory;
            }

            if (previews[selectedCategory].Count != items.Count)
            {
                GeneratePreviews();
            }
        }

        private void InitializeCategories()
        {
            categories = MetaDataManager.GetCategories();
            categoryLabels = categories.Keys.ToList();

            previews = new Dictionary<string, Dictionary<string, Texture2D>>();
            foreach (string category in categories.Keys)
            {
                previews.Add(category, new Dictionary<string, Texture2D>());
            }

            selectedCategory =  categories.Keys.ToList()[0];
            items = categories[selectedCategory];
            GeneratePreviews();
            Repaint();
        }

        private void LoadTexture() 
        {
            defaultPreview = AssetDatabase.LoadAssetAtPath<Texture2D>("Packages/com.ucm.editorniveles/Assets/DefaultPreview.png");
        }

        private void DrawTabs()
        {
            int index = categories.Keys.ToList().IndexOf(selectedCategory);
            index = GUILayout.Toolbar(index, categoryLabels.ToArray());
            selectedCategory = categoryLabels[index]; //[index];
        }

        private void DrawRefreshButton() 
        {
            float padding = 10f;
            float buttonWidth = 100f;
            float buttonHeight = 30f;

            Rect buttonRect = new Rect(
                position.width - buttonWidth - padding,
                position.height - buttonHeight - padding,
                buttonWidth,
                buttonHeight
            );

            // Make sure layout space is reserved
            GUILayout.Space(buttonHeight + padding * 2);

            // Use GUI instead of GUILayout for absolute positioning
            if (GUI.Button(buttonRect, "Refresh"))
            {
                MetaDataManager.InitializeCategories();
                InitializeCategories();
                GeneratePreviews();
            }
        }

        private void OnGUI()
        {
            DrawTabs();
            DrawScroll();
            DrawRefreshButton();
        }
        private GUIContent[] GetGUIContentsFromItems()
        {
            List<GUIContent> guiContents = new List<GUIContent>();
            if (previews[selectedCategory].Count == items.Count)
            {
                int totalItems = items.Count;
                for (int i = 0; i < totalItems; i++)
                {
                    GUIContent guiContent = new GUIContent();
                    Texture2D preview;
                    guiContent.text = items[i].name;
                    if (previews[selectedCategory].TryGetValue(items[i].name, out preview))
                    {
                        guiContent.image = preview;
                    }
                    guiContents.Add(guiContent);
                }
            }
            return guiContents.ToArray();
        }
        private GUIStyle GetGUIStyle()
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.alignment = TextAnchor.LowerCenter;
            guiStyle.imagePosition = ImagePosition.ImageAbove;
            guiStyle.fixedWidth = buttonWidth;
            guiStyle.fixedHeight = buttonHeight;
            return guiStyle;
        }
        private void GetSelectedItem(int index)
        {
            if (index != -1)
            {
                GameObject selectedItem = items[index];
                if (ItemSelectedEvent != null) ItemSelectedEvent(selectedItem);
            }
        }
        private void DrawScroll()
        {
            int rowCapacity = Mathf.FloorToInt(position.width / (buttonWidth));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            int selectionGridIndex = -1;
            selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex,
            GetGUIContentsFromItems(), rowCapacity, GetGUIStyle());
            GetSelectedItem(selectionGridIndex);
            GUILayout.EndScrollView();
        }
        private void GeneratePreviews()
        {
            foreach (GameObject item in items)
            {
                if (!previews[selectedCategory].ContainsKey(item.name))
                {
                    Texture2D preview = AssetPreview.GetAssetPreview(item);
                    if (preview != null)
                    {
                        previews[selectedCategory].Add(item.name, preview);
                    }   

                    else if(defaultPreview != null) previews[selectedCategory].Add(item.name, defaultPreview);
                }
            }
        }
    }
}
