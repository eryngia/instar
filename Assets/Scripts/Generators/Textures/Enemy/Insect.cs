using System;
using System.Collections.Generic;
using UnityEngine;

// An entire procedural insect.
public class Insect {
    
    public InsectBody body;
    public InsectAppendages appendages;
    
    public Insect(InsectBody body, InsectAppendages appendages) {
        this.body = body;
        this.appendages = appendages;
    }

    public void Color(Color[] pixels, Color color, Color highlightColor) {
        body.Color(pixels, color);
        appendages.Color(pixels, color);
        ColorHighlights(pixels, highlightColor);
    }

    public void Color1(Color[] pixels, Color color, Color highlightColor) {
        body.Color1(pixels, color);
        appendages.Color1(pixels, color);
        ColorHighlights1(pixels, highlightColor);
    }

    public void Color2(Color[] pixels, Color color, Color highlightColor) {
        body.Color2(pixels, color);
        appendages.Color2(pixels, color);
        ColorHighlights2(pixels, highlightColor);
    }

    public void ColorHighlights(Color[] pixels, Color color) {
        body.ColorHighlights(pixels, color);
        appendages.ColorHighlights(pixels, color);
    }

    public void ColorHighlights1(Color[] pixels, Color color) {
        body.ColorHighlights1(pixels, color);
        appendages.ColorHighlights1(pixels, color);
    }

    public void ColorHighlights2(Color[] pixels, Color color) {
        body.ColorHighlights2(pixels, color);
        appendages.ColorHighlights2(pixels, color);
    }


    public static Insect Generate(int pixelSize, int highlightCount) {
        // create parameters
        List<InsectParameter> parameters = InsectParameters.Generate(highlightCount);
        Debug.Log("Generating insect with parameters: " + string.Join(", ", parameters));

        // determine body size baed on number of parts
        float bodySize;
        if (parameters.Contains(InsectParameter.TwoParts))
          bodySize = RandomUtils.RangeAvg(0.3f, 0.6f);
        else if (parameters.Contains(InsectParameter.ThreeParts)) 
            bodySize = RandomUtils.RangeAvg(0.4f, 0.7f);
        else if (parameters.Contains(InsectParameter.ManyParts))
            bodySize = RandomUtils.RangeAvg(0.6f, 0.8f);
        else
            throw new Exception("Illegal parameter combination: " + string.Join(",", parameters));

        // generate body and appendages
        InsectBody body = InsectBody.Generate(pixelSize, bodySize, parameters);
        InsectAppendages appendages = InsectAppendages.Generate(pixelSize, bodySize, parameters, body.bodyParts);
        
        return new Insect(body, appendages);
    }

    public int GetWidth() {
        return 2 * GetSide();
    }

    public int GetHeight() {
        return GetBottom() - GetTop();
    }

    public int GetTop() {
        return Mathf.Min(body.GetTop(), appendages.GetTop());
    }

    public int GetBottom() {
        return Mathf.Max(body.GetBottom(), appendages.GetBottom());
    }

    public int GetSide() {
        return Mathf.Max(body.GetSide(), appendages.GetSide());
    }

}