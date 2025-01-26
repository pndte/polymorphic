Shader "Custom/SDFMorphFX_Production"
{
    Properties
    {
        // --- SDF-текстуры (одноканальные) ---
        _SDF1       ("SDF A", 2D) = "white" {}
        _SDF2       ("SDF B", 2D) = "white" {}
        
        // --- Оригинальные цветные текстуры ---
        _MainTex1   ("Sprite A (Color Tex)", 2D) = "white" {}
        _MainTex2   ("Sprite B (Color Tex)", 2D) = "white" {}
        
        // Параметр перехода 0..1 (управляем скриптом)
        _Transition ("Transition 0..1", Range(0,1)) = 0
        
        // --- Параметры шумовых бликов ---
        _NoiseTex   ("Noise Tex (for highlights)", 2D) = "white" {}
        _HighlightColor ("Highlight Color", Color) = (1,1,1,1)
        _HighlightIntensity ("Highlight Intensity", Range(0,5)) = 1
        
        // --- Параметры обводки ---
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineThickness ("Outline Thickness (SDF units)", Range(0,0.2)) = 0.05
        _OutlinePulseSpeed("Outline Pulse Speed", Range(0,20)) = 5.0
        _OutlineIntensity ("Outline Intensity", Range(0,2)) = 1.0
    }
    
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100
        
        // Прозрачный блендинг
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            // Для fwidth лучше target 3.0+
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            //====================//
            //   Структуры       //
            //====================//
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv      : TEXCOORD0;
                float4 vertex  : SV_POSITION;
            };
            
            //====================================================//
            //            Переменные-шейдеры (samplers)           //
            //====================================================//
            
            // -- SDF --
            sampler2D _SDF1;
            float4    _SDF1_ST; // если нужно TRANSFORM_TEX
            sampler2D _SDF2;
            float4    _SDF2_ST;
            
            // -- Original Textures --
            sampler2D _MainTex1;
            float4    _MainTex1_ST;
            sampler2D _MainTex2;
            float4    _MainTex2_ST;
            
            // -- Noise --
            sampler2D _NoiseTex;
            float4    _NoiseTex_ST;
            
            // Параметры
            float  _Transition;
            
            float4 _HighlightColor;
            float  _HighlightIntensity;
            
            float4 _OutlineColor;
            float  _OutlineThickness;
            float  _OutlineIntensity;
            float  _OutlinePulseSpeed;
            
            //====================================================//
            //                 Вершинный шейдер                   //
            //====================================================//
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Для удобства, допустим, что UV совпадает. 
                // Если нужно применять Tiling/Offset, используйте TRANSFORM_TEX.
                o.uv = v.uv;
                return o;
            }
            
            //====================================================//
            //            Вспомогательные функции                 //
            //====================================================//
            
            // Плавный переход через чёрный (A->Black->B)
            // Возвращает {color, alpha}.
            float4 ColorTransition(float4 colA, float4 colB, float t)
            {
                // t [0..1], при 0..0.5 => (A->Black), при 0.5..1 => (Black->B)
                float4 black = float4(0,0,0,1);
                
                if (t < 0.5)
                {
                    float f = t * 2.0;
                    // Цвет
                    float3 c = lerp(colA.rgb, black.rgb, f);
                    // Альфа (если хотим тоже "в середине" = 1)
                    float a = lerp(colA.a, 1.0, f);
                    return float4(c, a);
                }
                else
                {
                    float f = (t - 0.5) * 2.0;
                    float3 c = lerp(black.rgb, colB.rgb, f);
                    float a = lerp(1.0, colB.a, f);
                    return float4(c, a);
                }
            }
            
            //====================================================//
            //               Фрагментный шейдер                   //
            //====================================================//
            fixed4 frag (v2f i) : SV_Target
            {
                //------------------------------------------
                // 1) Берём distanceA, distanceB из SDF-текстур
                //------------------------------------------
                // Предположим, что SDF упакован в канал R как [0..1], 
                // где 0.5 ~ граница, а (0..0.5) внутри, (0.5..1) снаружи 
                // или другой вариант. Часто удобнее [0..1] -> [-1..+1].
                // Далее — зависит от того, как вы упаковали SDF. 
                // Примерно так:
                half dA = tex2D(_SDF1, i.uv).r * 2.0 - 1.0;
                half dB = tex2D(_SDF2, i.uv).r * 2.0 - 1.0;
                
                // Линейный морф
                half shapeT = _Transition;
                half distance = lerp(dA, dB, shapeT);
                
                // Для сглаживания края используем fwidth(distance).
                half edgeWidth = fwidth(distance);
                
                //------------------------------------------
                // 2) Определим альфу по SDF (AntiAlias)
                //------------------------------------------
                // Классический приём: внутри => distance < 0,
                // но вместо жёсткого отсечения берём smoothstep.
                // Чем выше edgeWidth, тем мягче граница при масштабировании.
                // 0.0 в аргументах можно сдвигать, если хотите «размытый» край.
                
                // Внутри (distance < 0) => alpha=1, 
                // Снаружи (distance > 0) => alpha=0.
                // Порог = 0, растянутый на edgeWidth:
                half alphaSDF = saturate(smoothstep(0.0, -edgeWidth, distance));
                
                if (alphaSDF <= 0.001h)
                {
                    // Пиксель снаружи, можно возвращать прозрачность
                    return 0;
                }
                
                //------------------------------------------
                // 3) Берём два цветовых спрайта, 
                //    интерполируем (A->Black->B)
                //------------------------------------------
                // В идеале обе текстуры (MainTex1/2) имеют одинаковый размер
                // и согласованные UV. Тогда просто делаем:
                
                float4 colA = tex2D(_MainTex1, i.uv);
                float4 colB = tex2D(_MainTex2, i.uv);
                
                // Функция: A->black->B (по тому же _Transition)
                float4 colorTransition = ColorTransition(colA, colB, _Transition);
                
                // Но учтём ещё SDF-шную "выключку" альфы:
                // то есть, если исходная текстура A имела alpha < 1, 
                // тоже можно учесть. Однако обычно SDF уже задаёт нам форму.
                // Если хотим — можно умножить:
                float finalAlpha = colorTransition.a * alphaSDF;
                float3 finalRGB  = colorTransition.rgb;
                
                //------------------------------------------
                // 4) Шумовые блики (в середине t=0.5)
                //------------------------------------------
                // Берём noise:
                float2 noiseUV = i.uv + float2(_Time.y * 0.1, _Time.y * 0.13);
                float noiseVal = tex2D(_NoiseTex, noiseUV).r;
                
                // Маска, усиливающаяся при t=0.5
                float highlightMask = 1.0 - abs(_Transition - 0.5) * 2.0; 
                highlightMask = saturate(highlightMask);
                
                // Порог, чтобы сделать "пятна"
                float threshold = 0.7; 
                float flash = step(threshold, noiseVal) * highlightMask * _HighlightIntensity;
                
                float3 highlightCol = _HighlightColor.rgb * flash;
                
                // Аддитивно добавим:
                finalRGB += highlightCol;
                
                //------------------------------------------
                // 5) Обводка по границе (Outline)
                //------------------------------------------
                // Если |distance| < _OutlineThickness => близко к контуру
                // Для плавности возьмём smoothstep вокруг этой зоны.
                
                half outlineRange = _OutlineThickness; // значение в диапазоне (0..0.2)
                half distAbs = abs(distance);
                
                // fwidth(distAbs) можно взять ~ edgeWidth
                // Сделаем плавный переход в зоне [outlineRange - edgeWidth, outlineRange + edgeWidth].
                half outlineMask = 1.0 - smoothstep(outlineRange - edgeWidth, outlineRange + edgeWidth, distAbs);
                
                // Пульсация:
                float pulse = sin(_Time.y * _OutlinePulseSpeed) * 0.5 + 0.5; // 0..1
                outlineMask *= pulse * _OutlineIntensity;
                
                // Смешаем цвет обводки с нашим finalRGB
                // Обычно делаем lerp: 
                finalRGB = lerp(finalRGB, _OutlineColor.rgb, outlineMask);
                
                //------------------------------------------
                // 6) Выдаём результат
                //------------------------------------------
                return float4(finalRGB, finalAlpha);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
