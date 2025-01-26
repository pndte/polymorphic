using System.IO;
using UnityEditor;
using UnityEngine;

namespace PInfrastructure.Utils
{
    public static class SDFGenerator
    {
        /// <summary>
        /// Генерация Signed Distance Field (одноканальной) текстуры из RGBA-спрайта.
        /// </summary>
        /// <param name="source">Исходная текстура (спрайт), где альфа указывает силуэт</param>
        /// <param name="maxDistance">Максимальный радиус (в пикселях) для расчёта distance</param>
        /// <returns>Готовая SDF-текстура размером, как у source</returns>
        public static Texture2D GenerateSDF(Texture2D source, int maxDistance = 256)
        {
            int w = source.width;
            int h = source.height;
        
            // Считаем альфу как "0 = снаружи, 1 = внутри"
            Color[] srcPixels = source.GetPixels();
            float[] alpha = new float[w * h];
            for (int i = 0; i < srcPixels.Length; i++)
            {
                alpha[i] = srcPixels[i].a > 0.5f ? 1f : 0f; 
                // можно и более градуально брать srcPixels[i].a
            }
        
            // Выходной массив distance
            float[] distanceField = new float[w * h];
        
            // Для каждого пикселя ищем расстояние до ближайшей границы (делаем brute force для простоты)
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    bool inside = (alpha[y * w + x] > 0.5f);
                
                    int minDistSq = maxDistance * maxDistance + 1; // квадрат расстояния
                    // Ищем пиксель "противоположной" принадлежности (если inside, то ищем пиксель снаружи, и наоборот).
                    // Но ещё проще — ищем пиксель, где alpha = 0.5 "грань" (или "другая сторона").
                
                    for (int dy = -maxDistance; dy <= maxDistance; dy++)
                    {
                        int yy = y + dy;
                        if (yy < 0 || yy >= h) continue;
                        for (int dx = -maxDistance; dx <= maxDistance; dx++)
                        {
                            int xx = x + dx;
                            if (xx < 0 || xx >= w) continue;
                        
                            bool otherInside = (alpha[yy * w + xx] > 0.5f);
                            if (otherInside != inside)
                            {
                                int distSq = dx * dx + dy * dy;
                                if (distSq < minDistSq)
                                    minDistSq = distSq;
                            }
                        }
                    }
                
                    float dist = Mathf.Sqrt(minDistSq);
                    // Подпишем знак: inside => отрицательное, outside => положительное
                    if (inside)
                        dist = -dist;
                
                    distanceField[y * w + x] = dist;
                }
            }
        
            // Нормируем distance в диапазон 0..1, чтобы сохранить в 8-битном канале
            // Найдём минимальный и максимальный distance
            float minVal = float.MaxValue;
            float maxVal = float.MinValue;
            for (int i = 0; i < distanceField.Length; i++)
            {
                if (distanceField[i] < minVal) minVal = distanceField[i];
                if (distanceField[i] > maxVal) maxVal = distanceField[i];
            }
            float range = maxVal - minVal;
        
            // Создаём новую текстуру
            Texture2D sdfTex = new Texture2D(w, h, TextureFormat.R8, false);
            Color[] sdfPixels = new Color[w * h];
        
            for (int i = 0; i < distanceField.Length; i++)
            {
                float norm = (distanceField[i] - minVal) / range; // 0..1
                // Запишем в красный канал
                sdfPixels[i] = new Color(norm, 0, 0, 1);
            }
        
            sdfTex.SetPixels(sdfPixels);
            sdfTex.Apply();
        
            return sdfTex;
        }

        /// <summary>
        /// Пример утилиты, создающей SDF-текстуру в Assets рядом с исходным спрайтом.
        /// </summary>
        [MenuItem("PM/Generate SDF From Sprite")]
        public static void GenerateSDFFromSelectedSprite()
        {
            if (Selection.activeObject is Texture2D source)
            {
                // Генерим
                var sdf = GenerateSDF(source, 64);
            
                // Сохраняем как PNG рядом
                string path = AssetDatabase.GetAssetPath(source);
                string dir = Path.GetDirectoryName(path);
                string fileName = Path.GetFileNameWithoutExtension(path);
                string newPath = Path.Combine(dir, fileName + "_SDF.png");
            
                File.WriteAllBytes(newPath, sdf.EncodeToPNG());
                AssetDatabase.Refresh();
                Debug.Log("SDF saved to: " + newPath);
            }
        }
    }
}
