using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// The body sections of a procedural insect.
public class InsectBody : InsectPart {
    
    
    public List<InsectBodyPart> bodyParts;
    public int eyes;
    
    public InsectBody(List<InsectBodyPart> bodyParts, int eyes) {
        this.bodyParts = bodyParts;
        this. eyes = eyes;
    }

    public override void Color(Color[] pixels, Color color) {
        foreach (var part in bodyParts) {
            part.Color(pixels, color);
        }
    }

    public override void Color1(Color[] pixels, Color color) {
        foreach (var part in bodyParts) {
            part.Color1(pixels, color);
        }
    }

    public override void Color2(Color[] pixels, Color color) {
        foreach (var part in bodyParts) {
            part.Color2(pixels, color);
        }
    }

    public override void ColorHighlights(Color[] pixels, Color color) {
        ColorEyes(pixels, UnityEngine.Color.red);
        foreach (var part in bodyParts) {
            part.ColorHighlights(pixels, color);
        }
    }

    public static InsectBody Generate(int pixelSize, float bodySize, List<InsectParameter> parameters) {
        List<InsectBodyPart> bodyParts;
        
        if (parameters.Contains(InsectParameter.TwoParts)) {
            bodyParts = GenerateTwoBodyParts(pixelSize, bodySize, parameters);
        } else if (parameters.Contains(InsectParameter.ThreeParts)) {
            bodyParts = GenerateThreeBodyParts(pixelSize, bodySize, parameters);
        } else if (parameters.Contains(InsectParameter.ManyParts)) {
            bodyParts = GenerateManyBodyParts(pixelSize, bodySize, parameters);
        } else {
            throw new Exception("Illegal parameter combination: " + string.Join(",", parameters));
        }

        int eyes;
        if (parameters.Contains(InsectParameter.EyesHighlight)) {
            eyes = parameters.Contains(InsectParameter.EightLegs) ? 4 : 2; // 8 legs means double eyes
        } else {
            eyes = 0;
        }

        return new InsectBody(bodyParts, eyes);
    }

    public static List<InsectBodyPart> GenerateTwoBodyParts(int pixelSize, float bodySize, List<InsectParameter> parameters) {
        
        InsectBodyPart head, thorax;
        bool outline = parameters.Contains(InsectParameter.BodyOutlineHighlight);
        int stripeWidth = parameters.Contains(InsectParameter.BodyStripesHighlight) ? UnityEngine.Random.Range(1, 3) : 0;
        int spotSize = parameters.Contains(InsectParameter.BodySpotsHighlight) ? 2 : 0;

        if (parameters.Contains(InsectParameter.ThinBody)) {
            head = InsectBodyPart.GenerateThin(pixelSize, (0.1f + RandomUtils.RangeAvg(0f, 0.2f)) * bodySize, TextureUtils.GetPixel(0, (2- bodySize) / 2, pixelSize), outline, stripeWidth, spotSize);
            thorax = InsectBodyPart.GenerateThin(pixelSize, bodySize - head.height, head.bottom, outline, stripeWidth, spotSize);

        } else if (parameters.Contains(InsectParameter.FlatBody)) {
            head = InsectBodyPart.GenerateFlat(pixelSize, (0.1f + RandomUtils.RangeAvg(0f, 0.3f)) * bodySize, TextureUtils.GetPixel(0, (2- bodySize) / 2, pixelSize), outline, stripeWidth, spotSize);
            thorax = InsectBodyPart.GenerateRound(pixelSize, bodySize - head.height, head.bottom, outline, stripeWidth, spotSize);

        } else if (parameters.Contains(InsectParameter.RoundBody)) {
            head = InsectBodyPart.GenerateRound(pixelSize, (0.1f + RandomUtils.RangeAvg(0f, 0.3f)) * bodySize, TextureUtils.GetPixel(0, (2- bodySize) / 2, pixelSize), outline, stripeWidth, spotSize);
            thorax = InsectBodyPart.GenerateRound(pixelSize, bodySize - head.height, head.bottom, outline, stripeWidth, spotSize);

        } else {
            throw new Exception("Illegal parameter combination: " + string.Join(",", parameters));
        }

        return new List<InsectBodyPart> {head, thorax};
    }

    public static List<InsectBodyPart> GenerateThreeBodyParts(int pixelSize, float bodySize, List<InsectParameter> parameters) {
        
        InsectBodyPart head, thorax, abdomen;
        bool outline = parameters.Contains(InsectParameter.BodyOutlineHighlight);
        int stripeWidth = parameters.Contains(InsectParameter.BodyStripesHighlight) ? UnityEngine.Random.Range(1, 3) : 0;
        int spotSize = parameters.Contains(InsectParameter.BodySpotsHighlight) ? 2 : 0;

        if (parameters.Contains(InsectParameter.ThinBody)) {
            head = InsectBodyPart.GenerateThin(pixelSize, (0.1f + RandomUtils.RangeAvg(0f, 0.2f)) * bodySize, TextureUtils.GetPixel(0, (2- bodySize) / 2, pixelSize), outline, stripeWidth, spotSize);
            thorax = InsectBodyPart.GenerateThin(pixelSize, (0.25f + RandomUtils.RangeAvg(0f, 0.2f)) * bodySize, head.bottom, outline, stripeWidth, spotSize);
            abdomen = InsectBodyPart.GenerateThin(pixelSize, bodySize - head.height - thorax.height, thorax.bottom, outline, stripeWidth, spotSize);

        } else if (parameters.Contains(InsectParameter.FlatBody)) {
            head = InsectBodyPart.GenerateFlat(pixelSize, (0.1f + RandomUtils.RangeAvg(0f, 0.1f)) * bodySize, TextureUtils.GetPixel(0, (2- bodySize) / 2, pixelSize), outline, stripeWidth, spotSize);
            thorax = InsectBodyPart.GenerateFlat(pixelSize, (0.2f + RandomUtils.RangeAvg(0f, 0.15f)) * bodySize, head.bottom, outline, stripeWidth, spotSize);
            abdomen = InsectBodyPart.GenerateRound(pixelSize, bodySize - head.height - thorax.height, thorax.bottom, outline, stripeWidth, spotSize);

        } else if (parameters.Contains(InsectParameter.RoundBody)) {
            head = InsectBodyPart.GenerateRound(pixelSize, (0.1f + RandomUtils.RangeAvg(0f, 0.2f)) * bodySize, TextureUtils.GetPixel(0, (2- bodySize) / 2, pixelSize), outline, stripeWidth, spotSize);
            thorax = InsectBodyPart.GenerateRound(pixelSize, (0.25f + RandomUtils.RangeAvg(0f, 0.2f)) * bodySize, head.bottom, outline, stripeWidth, spotSize);
            abdomen = InsectBodyPart.GenerateRound(pixelSize, bodySize - head.height - thorax.height, thorax.bottom, outline, stripeWidth, spotSize);

        } else {
            throw new Exception("Illegal parameter combination: " + string.Join(",", parameters));
        }

        return new List<InsectBodyPart> {head, thorax, abdomen};
    }

    public static List<InsectBodyPart> GenerateManyBodyParts(int pixelSize, float bodySize, List<InsectParameter> parameters) {
        int parts = UnityEngine.Random.Range(4, 6);
        float height = bodySize / parts;
        bool outline = parameters.Contains(InsectParameter.BodyOutlineHighlight);
        int stripeWidth = parameters.Contains(InsectParameter.BodyStripesHighlight) ? UnityEngine.Random.Range(1, 3) : 0;
        int spotSize = parameters.Contains(InsectParameter.BodySpotsHighlight) ? 2 : 0;
        List<InsectBodyPart> bodyParts = new  List<InsectBodyPart>();
        for (int i = 0; i < parts; i++) {
            bodyParts.Add(InsectBodyPart.GenerateRound(pixelSize, height, TextureUtils.GetPixel(0, (2- bodySize) / 2 + i * height, pixelSize), outline, stripeWidth, spotSize));
        }
        return bodyParts;
    }

    private void ColorEyes(Color[] pixels, Color color) {
        if (eyes == 0) return;
        
        Vector2 eye1 = new Vector2(1f, bodyParts[0].top.y + 1f);
        TextureUtils.ColorPixelMirrored(pixels, eye1, color);

        if (eyes == 4) {
            Vector2 eye2 = eye1 + new Vector2(1f, 1f);
            TextureUtils.ColorPixelMirrored(pixels, eye2, color);
        }        
    }

    public override int GetTop() {
        return bodyParts.Min(a => a.GetTop());
    }

    public override int GetBottom() {
        return bodyParts.Max(a => a.GetBottom());
    }

    public override int GetSide() {
        return bodyParts.Max(a => a.GetSide());
    }

}