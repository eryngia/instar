using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// A collection of procedural insect appendages.
public class InsectAppendages : InsectPart {
    
    
    public List<InsectAppendage> appendages;
    
    public InsectAppendages(List<InsectAppendage> appendages) {
        this.appendages = appendages;
    }

    public override void Color(Color[] pixels, Color color) {
        foreach (InsectAppendage appendage in appendages) {
            appendage.Color(pixels, color);
        }
    }

    public override void Color1(Color[] pixels, Color color) {
        foreach (InsectAppendage appendage in appendages) {
            appendage.Color1(pixels, color);
        }
    }

    public override void Color2(Color[] pixels, Color color) {
        foreach (InsectAppendage appendage in appendages) {
            appendage.Color2(pixels, color);
        }
    }

    public override void ColorHighlights(Color[] pixels, Color color) {
        foreach (InsectAppendage appendage in appendages) {
            appendage.ColorHighlights(pixels, color);
        }
    }

    public override void ColorHighlights1(Color[] pixels, Color color) {
        foreach (InsectAppendage appendage in appendages) {
            appendage.ColorHighlights1(pixels, color);
        }
    }

    public override void ColorHighlights2(Color[] pixels, Color color) {
        foreach (InsectAppendage appendage in appendages) {
            appendage.ColorHighlights2(pixels, color);
        }
    }

    public static InsectAppendages Generate(int pixelSize, float bodySize, List<InsectParameter> parameters, List<InsectBodyPart> bodyParts) {
        List<InsectAppendage> appendages = new List<InsectAppendage>();
        
        // generate legs, wings, head appendages depending on paramters
        if (!parameters.Contains(InsectParameter.NoLegs)) 
            appendages.AddRange(GenerateLegs(pixelSize, bodySize, parameters, bodyParts));
        if (parameters.Contains(InsectParameter.Wings)) 
            appendages.AddRange(GenerateWings(pixelSize, bodySize, parameters, bodyParts));
        if (!parameters.Contains(InsectParameter.NoHeadAppendage)) 
            appendages.AddRange(GenerateHeadAppendages(pixelSize, bodySize, parameters, bodyParts[0]));

        return new InsectAppendages(appendages);
    }

    public static InsectAppendage[] GenerateLegs(int pixelSize, float bodySize, List<InsectParameter> parameters, List<InsectBodyPart> body) {
        if (parameters.Contains(InsectParameter.JointedLegs)) {
            return GenerateJointedLegs(pixelSize, bodySize, parameters, body);
        } else if (parameters.Contains(InsectParameter.CurvedLegs)) {
            return GenerateCurvedLegs(pixelSize, bodySize, parameters, body);
        } else {
            throw new Exception("Illegal parameter combination: " + string.Join(",", parameters));
        }
    }

    public static InsectAppendage[] GenerateJointedLegs(int pixelSize, float bodySize, List<InsectParameter> parameters, List<InsectBodyPart> bodyParts) {
        InsectBodyPart bodyPart = bodyParts[1];
        float width = 0.075f + RandomUtils.RangeAvg(0f, 0.1f); 
        float height = 0.2f + RandomUtils.RangeAvg(0f, 0.15f);
        if (parameters.Contains(InsectParameter.Wings)) height /= 2; // half length legs if wings present
        float jointPoint = RandomUtils.RangeAvg(0.3f, 0.7f);
        Vector2 jointOffsetFront = new Vector2(Mathf.Sign(width) * RandomUtils.RangeAvg(0f, 4f), Mathf.Sign(height) * RandomUtils.RangeAvg(0f, 4f));
        Vector2 jointOffsetBack = new Vector2(jointOffsetFront.x, -jointOffsetFront.y);
        float animation = parameters.Contains(InsectParameter.Wings) ? 0f : RandomUtils.RangeAvg(0.5f, 1.5f); // no animation if wings present
        bool highlighted = parameters.Contains(InsectParameter.LegHighlight);
        
        if (parameters.Contains(InsectParameter.SixLegs)) {
            List<Vector2> anchors = TextureUtils.PointsOnCurves(bodyPart.top, bodyPart.topCurve, bodyPart.middle, bodyPart.bottomCurve, bodyPart.bottom, new[] {0.25f, 0.5f, 0.75f});
            var leg1 = InsectLeg.GenerateJointed(pixelSize, anchors[0], width, height, jointPoint, jointOffsetFront, animation, highlighted);
            var leg2 = InsectLeg.GenerateJointed(pixelSize, anchors[1], width * 1.4f, -height * 0.8f, jointPoint, jointOffsetBack, animation, highlighted);
            var leg3 = InsectLeg.GenerateJointed(pixelSize, anchors[2], width, -height, jointPoint, jointOffsetBack, animation, highlighted);
            return new[] {leg1, leg2, leg3};

        } else if (parameters.Contains(InsectParameter.EightLegs)) {
            List<Vector2> anchors = TextureUtils.PointsOnCurves(bodyPart.top, bodyPart.topCurve, bodyPart.middle, bodyPart.bottomCurve, bodyPart.bottom, new[] {0.2f, 0.4f, 0.6f, 0.8f});
            var leg1 = InsectLeg.GenerateJointed(pixelSize, anchors[0], width, height, jointPoint, jointOffsetFront, animation, highlighted);
            var leg2 = InsectLeg.GenerateJointed(pixelSize, anchors[1], width * 1.4f, height * 0.8f, jointPoint, jointOffsetFront, animation, highlighted);
            var leg3 = InsectLeg.GenerateJointed(pixelSize, anchors[2], width * 1.4f, -height * 0.8f, jointPoint, jointOffsetBack, animation, highlighted);
            var leg4 = InsectLeg.GenerateJointed(pixelSize, anchors[3], width, -height, jointPoint, jointOffsetBack, animation, highlighted);
            return new[] {leg1, leg2, leg3, leg4};

        } else if (parameters.Contains(InsectParameter.ManyLegs)) {
            InsectLeg[] legs = new InsectLeg[bodyParts.Count];
            for (int i = 0; i < bodyParts.Count; i++) {
                legs[i] = InsectLeg.GenerateJointed(pixelSize, bodyParts[i].middle, width / 2, -height / 2, jointPoint, jointOffsetBack / 2, animation / 2, highlighted);
            }
            return legs;

        } else {
            throw new Exception("Illegal parameter combination: " + string.Join(",", parameters));
        }
    }

    public static InsectAppendage[] GenerateCurvedLegs(int pixelSize, float bodySize, List<InsectParameter> parameters, List<InsectBodyPart> bodyParts) {
        InsectBodyPart bodyPart = bodyParts[1];
        float width = 0.1f + RandomUtils.RangeAvg(0f, 0.15f); 
        float height = 0.2f + RandomUtils.RangeAvg(0f, 0.15f);
        float curve = RandomUtils.RangeAvg(2.5f, 4f);
        if (parameters.Contains(InsectParameter.Wings)) height /= 2; // half length legs if wings present
        float animation = parameters.Contains(InsectParameter.Wings) ? 0f : RandomUtils.RangeAvg(0.5f, 1.5f); // no animation if wings present
        bool highlighted = parameters.Contains(InsectParameter.LegHighlight);

        if (parameters.Contains(InsectParameter.SixLegs)) {
            List<Vector2> anchors = TextureUtils.PointsOnCurves(bodyPart.top, bodyPart.topCurve, bodyPart.middle, bodyPart.bottomCurve, bodyPart.bottom, new[] {0.25f, 0.5f, 0.75f});
            var leg1 = InsectLeg.GenerateCurved(pixelSize, anchors[0], width, height, -curve, animation, highlighted);
            var leg2 = InsectLeg.GenerateCurved(pixelSize, anchors[1], width * 1.4f, -height * 0.8f, curve, animation, highlighted);
            var leg3 = InsectLeg.GenerateCurved(pixelSize, anchors[2], width, -height, curve, animation, highlighted);
            return new[] {leg1, leg2, leg3};

        } else if (parameters.Contains(InsectParameter.EightLegs)) {
            List<Vector2> anchors = TextureUtils.PointsOnCurves(bodyPart.top, bodyPart.topCurve, bodyPart.middle, bodyPart.bottomCurve, bodyPart.bottom, new[] {0.25f, 0.5f, 0.5f, 0.75f});
            var leg1 = InsectLeg.GenerateCurved(pixelSize, anchors[0], width, height, -curve, animation, highlighted);
            var leg2 = InsectLeg.GenerateCurved(pixelSize, anchors[1], width * 1.4f, height * 0.8f, -curve, animation, highlighted);
            var leg3 = InsectLeg.GenerateCurved(pixelSize, anchors[2], width * 1.4f, -height * 0.8f, curve, animation, highlighted);
            var leg4 = InsectLeg.GenerateCurved(pixelSize, anchors[3], width, -height, curve, animation, highlighted);
            return new[] {leg1, leg2, leg3, leg4};

        } else if (parameters.Contains(InsectParameter.ManyLegs)) {
            InsectLeg[] legs = new InsectLeg[bodyParts.Count];
            for (int i = 0; i < bodyParts.Count; i++) {
                legs[i] = InsectLeg.GenerateCurved(pixelSize, bodyParts[i].middle, width / 2, -height / 2, curve / 2, animation / 2, highlighted);
            }
            return legs;

        } else {
            throw new Exception("Illegal parameter combination: " + string.Join(",", parameters));
        }
    }

    public static InsectAppendage[] GenerateWings(int pixelSize, float bodySize, List<InsectParameter> parameters, List<InsectBodyPart> body) {

        if (parameters.Contains(InsectParameter.ThinWings)) {
            return GenerateThinWings(pixelSize, bodySize, parameters, body);
        } else if (parameters.Contains(InsectParameter.BigWings)) {
            return GenerateBigWings(pixelSize, bodySize, parameters, body);
        } else if (parameters.Contains(InsectParameter.TriangleWings)) {
            return GenerateTriangleWings(pixelSize, bodySize, parameters, body);
        } else {
            throw new Exception("Illegal parameter combination: " + string.Join(",", parameters));
        }
    }

    public static InsectAppendage[] GenerateThinWings(int pixelSize, float bodySize, List<InsectParameter> parameters, List<InsectBodyPart> body) {
        Vector2 anchor = TextureUtils.PointOnCurve(body[1].top, body[1].middle, body[1].topCurve, 0.5f);
        bool outline = parameters.Contains(InsectParameter.WingOutlineHighlight);
        int spotSize = parameters.Contains(InsectParameter.WingSpotsHighlight) ? 2 : 0;

        InsectWing wing1 = InsectWing.GenerateThin(pixelSize, anchor, 0.35f + RandomUtils.RangeAvg(0f, 0.15f), 0.1f, 2f, outline, spotSize);
        InsectWing wing2 = InsectWing.GenerateThin(pixelSize, anchor, 0.35f + RandomUtils.RangeAvg(0f, 0.15f), -0.1f, 2f, outline, spotSize);

        return new[] {wing1, wing2};
    }
    
    public static InsectAppendage[] GenerateBigWings(int pixelSize, float bodySize, List<InsectParameter> parameters, List<InsectBodyPart> body) {
        Vector2 anchor = TextureUtils.PointOnCurve(body[1].top, body[1].middle, body[1].topCurve, 0.75f);
        float width = 0.25f + RandomUtils.RangeAvg(0f, 0.15f);
        float height = 0.15f + RandomUtils.RangeAvg(0f, 0.15f);
        float curve = RandomUtils.RangeAvg(5f, 9f);
        bool outline = parameters.Contains(InsectParameter.WingOutlineHighlight);
        int spotSize = parameters.Contains(InsectParameter.WingSpotsHighlight) ? 2 : 0;

        if (parameters.Contains(InsectParameter.SideWings) && parameters.Contains(InsectParameter.FourWings)) {
            InsectWing wing1 = InsectWing.GenerateBig(pixelSize, anchor, width, height, curve, outline, spotSize);
            InsectWing wing2 = InsectWing.GenerateBig(pixelSize, anchor, 1.3f * width, -1.3f * height, curve, outline, spotSize);
            return new[] {wing1, wing2};

        } else if (parameters.Contains(InsectParameter.SideWings) && parameters.Contains(InsectParameter.TwoWings)) {
            return new[] {InsectWing.GenerateBig(pixelSize, anchor, width * 1.5f, 0, curve * 1.5f, outline, spotSize)};

        } else if (parameters.Contains(InsectParameter.BackWings) && parameters.Contains(InsectParameter.TwoWings)) {
            return new[] {InsectWing.GenerateBig(pixelSize, anchor, 0.25f + RandomUtils.RangeAvg(0f, 0.15f), -0.25f + -RandomUtils.RangeAvg(0f, 0.15f), curve, outline, spotSize)};

        } else {
            throw new Exception("Illegal parameter combination: " + string.Join(",", parameters));
        }
    }

    public static InsectAppendage[] GenerateTriangleWings(int pixelSize, float bodySize, List<InsectParameter> parameters, List<InsectBodyPart> body) {
        Vector2 anchor = new Vector2(0f, TextureUtils.PointOnCurve(body[1].top, body[1].middle, body[1].topCurve, 0.5f).y);
        bool outline = parameters.Contains(InsectParameter.WingOutlineHighlight);
        int spotSize = parameters.Contains(InsectParameter.WingSpotsHighlight) ? 2 : 0;
        return new[] {InsectWing.GenerateTriangle(pixelSize, anchor, 0.4f + RandomUtils.RangeAvg(0f, 0.15f), -0.4f - RandomUtils.RangeAvg(0f, 0.15f), outline, spotSize)};
    }
    

    public static InsectAppendage[] GenerateHeadAppendages(int pixelSize, float bodySize, List<InsectParameter> parameters, InsectBodyPart head) {
        if (parameters.Contains(InsectParameter.Horns)) {
            bool highlighted = parameters.Contains(InsectParameter.HornHighlight);
            return new[] {InsectHeadAppendage.GenerateHorn(pixelSize, TextureUtils.PointOnCurve(head.top, head.middle, head.topCurve, 0.25f), highlighted)};

        } else if (parameters.Contains(InsectParameter.Antennae)) {
            bool highlighted = parameters.Contains(InsectParameter.AntennaHighlight);
            return new[] {InsectHeadAppendage.GenerateAntenna(pixelSize, TextureUtils.PointOnCurve(head.top, head.middle, head.topCurve, 0.25f), highlighted)};
        } else {
            throw new Exception("Illegal parameter combination: " + string.Join(",", parameters));
        }
    }

    public override int GetTop() {
        return appendages.Min(a => a.GetTop());
    }

    public override int GetBottom() {
        return appendages.Max(a => a.GetBottom());
    }

    public override int GetSide() {
        return appendages.Max(a => a.GetSide());
    }


}