using System;
using System.Collections.Generic;

// Parameter for the generation of a procedural insect.
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class InsectParameterAttribute : Attribute {
    public float Weight { get; } // weight of parameter in population
    public HashSet<InsectParameter> Excludes { get; } // list of excluded parameters if this is present

    public InsectParameterAttribute(float weight, InsectParameter[] excludes = null) {
        Weight = weight;
        Excludes = excludes != null ? new HashSet<InsectParameter>(excludes) : new HashSet<InsectParameter>();
    }
}

public enum InsectParameter {
        [InsectParameter(1f, new[] {Horns, NoHeadAppendage})]  
        Antennae, 
        [InsectParameter(1f, new[] {NoLegs, Wings, FourWings, TwoWings, ThinWings, BigWings, TriangleWings, SideWings, BackWings, Antennae, NoHeadAppendage, ThinBody})]  
        Horns, 
        [InsectParameter(1f, new[] {Antennae, Horns})]
        NoHeadAppendage,
        [InsectParameter(1f, new[] {RoundBody, ThinBody, ManyParts, EightLegs, ManyLegs})]
        FlatBody, 
        [InsectParameter(1f, new[] {FlatBody, ThinBody})]
        RoundBody, 
        [InsectParameter(1f, new[] {FlatBody, RoundBody, NoWings, TwoWings, ManyParts, BackWings, JointedLegs, CurvedLegs, EightLegs, SixLegs, ManyLegs})]
        ThinBody, 
        [InsectParameter(1f, new[] {TwoParts, ManyParts, ManyLegs})]
        ThreeParts, 
        [InsectParameter(1f, new[] {ThreeParts, ManyParts, ManyLegs})]
        TwoParts, 
        [InsectParameter(0.3f, new[] {TwoParts, ThreeParts, Wings, FourWings, TwoWings, ThinWings, BigWings, TriangleWings, SideWings, BackWings, NoLegs, EightLegs, SixLegs})]
        ManyParts,
        [InsectParameter(2.5f, new[] {Wings, FourWings, TwoWings, ThinWings, BigWings, TriangleWings, SideWings, BackWings, NoLegs})]
        NoWings,
        [InsectParameter(1f, new[] {NoWings, EightLegs, ManyLegs})]
        Wings, 
        [InsectParameter(1f, new[] {TwoWings, TriangleWings, BackWings, EightLegs, ManyLegs})]
        FourWings, 
        [InsectParameter(1f, new[] {FourWings, ThinWings, EightLegs, ManyLegs})]
        TwoWings, 
        [InsectParameter(1f, new[] {BigWings, TriangleWings, BackWings, JointedLegs, CurvedLegs, EightLegs, SixLegs, ManyLegs })]
        ThinWings,
        [InsectParameter(1f, new[] {ThinWings, TriangleWings, EightLegs, ManyLegs})]
        BigWings, 
        [InsectParameter(1f, new[] {ThinWings, BigWings, EightLegs, ManyLegs, SideWings, JointedLegs, CurvedLegs,  EightLegs, SixLegs, ManyLegs})]
        TriangleWings, 
        [InsectParameter(1f, new[] {BackWings, EightLegs, ManyLegs})]
        SideWings, 
        [InsectParameter(1f, new[] {SideWings, EightLegs, ManyLegs})]
        BackWings, 
        [InsectParameter(1f, new[] {JointedLegs, CurvedLegs,  EightLegs, SixLegs, ManyLegs})]
        NoLegs,
        [InsectParameter(1f, new[] {CurvedLegs})]
        JointedLegs, 
        [InsectParameter(1f, new[] {JointedLegs})]
        CurvedLegs, 
        [InsectParameter(1f, new[] {SixLegs, ManyLegs})]
        EightLegs, 
        [InsectParameter(1f, new[] {EightLegs, ManyLegs})]
        SixLegs, 
        [InsectParameter(0.3f, new[] {SixLegs, EightLegs})]
        ManyLegs,
        [InsectParameter(1f, new[] {Antennae, NoHeadAppendage, AntennaHighlight})]
        HornHighlight,
        [InsectParameter(1f, new[] {Horns, NoHeadAppendage, HornHighlight})]
        AntennaHighlight,
        [InsectParameter(1f, new InsectParameter[0])]
        EyesHighlight,
        [InsectParameter(1f, new[] {NoLegs})]
        LegHighlight,
        [InsectParameter(1f, new[] {BodyOutlineHighlight, BodyStripesHighlight})]
        BodySpotsHighlight,
        [InsectParameter(1f, new [] {NoWings, WingOutlineHighlight})]
        WingSpotsHighlight,
        [InsectParameter(1f, new[] {BodyStripesHighlight, BodySpotsHighlight, TriangleWings})]
        BodyOutlineHighlight,
        [InsectParameter(1f, new [] {NoWings, WingSpotsHighlight})]
        WingOutlineHighlight,
        [InsectParameter(1f, new[] {BodyOutlineHighlight, BodySpotsHighlight})]
        BodyStripesHighlight
}
