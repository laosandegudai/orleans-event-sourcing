// <auto-generated />
#if !EXCLUDE_CODEGEN
#pragma warning disable 162
#pragma warning disable 219
#pragma warning disable 414
#pragma warning disable 618
#pragma warning disable 649
#pragma warning disable 693
#pragma warning disable 1591
#pragma warning disable 1998
[assembly: global::Orleans.Metadata.FeaturePopulatorAttribute(typeof(OrleansGeneratedCode.OrleansCodeGen2a0594da8dFeaturePopulator))]
[assembly: global::System.CodeDom.Compiler.GeneratedCodeAttribute(@"Orleans-CodeGenerator", @"2.0.0.0")]
[assembly: global::Orleans.CodeGeneration.OrleansCodeGenerationTargetAttribute(@"Grains, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
namespace OrleansGeneratedCode3AC95144
{
    using global::Orleans;
    using global::System.Reflection;
}

namespace OrleansGeneratedCode
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(@"Orleans-CodeGenerator", @"2.0.0.0")]
    internal sealed class OrleansCodeGen2a0594da8dFeaturePopulator : global::Orleans.Metadata.IFeaturePopulator<global::Orleans.Metadata.GrainInterfaceFeature>, global::Orleans.Metadata.IFeaturePopulator<global::Orleans.Metadata.GrainClassFeature>, global::Orleans.Metadata.IFeaturePopulator<global::Orleans.Serialization.SerializerFeature>
    {
        public void Populate(global::Orleans.Metadata.GrainInterfaceFeature feature)
        {
        }

        public void Populate(global::Orleans.Metadata.GrainClassFeature feature)
        {
            feature.Classes.Add(new global::Orleans.Metadata.GrainClassMetadata(typeof(global::Grains.ValueGrain)));
        }

        public void Populate(global::Orleans.Serialization.SerializerFeature feature)
        {
            feature.AddKnownType(@"System.Action`9,System.Runtime", @"Action`9'9");
            feature.AddKnownType(@"System.Action`10,System.Runtime", @"Action`10'10");
            feature.AddKnownType(@"System.Action`11,System.Runtime", @"Action`11'11");
            feature.AddKnownType(@"System.Action`12,System.Runtime", @"Action`12'12");
            feature.AddKnownType(@"System.Action`13,System.Runtime", @"Action`13'13");
            feature.AddKnownType(@"System.Action`14,System.Runtime", @"Action`14'14");
            feature.AddKnownType(@"System.Action`15,System.Runtime", @"Action`15'15");
            feature.AddKnownType(@"System.Action`16,System.Runtime", @"Action`16'16");
            feature.AddKnownType(@"System.Func`10,System.Runtime", @"Func`10'10");
            feature.AddKnownType(@"System.Func`11,System.Runtime", @"Func`11'11");
            feature.AddKnownType(@"System.Func`12,System.Runtime", @"Func`12'12");
            feature.AddKnownType(@"System.Func`13,System.Runtime", @"Func`13'13");
            feature.AddKnownType(@"System.Func`14,System.Runtime", @"Func`14'14");
            feature.AddKnownType(@"System.Func`15,System.Runtime", @"Func`15'15");
            feature.AddKnownType(@"System.Func`16,System.Runtime", @"Func`16'16");
            feature.AddKnownType(@"System.Func`17,System.Runtime", @"Func`17'17");
            feature.AddKnownType(@"System.Lazy`2,System.Runtime", @"Lazy`2'2");
            feature.AddKnownType(@"System.Threading.WaitHandleExtensions,System.Runtime", @"WaitHandleExtensions");
            feature.AddKnownType(@"System.Runtime.AssemblyTargetedPatchBandAttribute,System.Runtime", @"AssemblyTargetedPatchBandAttribute");
            feature.AddKnownType(@"System.Runtime.TargetedPatchingOptOutAttribute,System.Runtime", @"TargetedPatchingOptOutAttribute");
            feature.AddKnownType(@"System.Runtime.ConstrainedExecution.PrePrepareMethodAttribute,System.Runtime", @"PrePrepareMethodAttribute");
            feature.AddKnownType(@"System.Reflection.RuntimeReflectionExtensions,System.Runtime", @"RuntimeReflectionExtensions");
            feature.AddKnownType(@"System.IO.FileAttributes,System.Runtime", @"FileAttributes");
            feature.AddKnownType(@"System.IO.HandleInheritability,System.Runtime", @"HandleInheritability");
            feature.AddKnownType(@"Grains.ValueGrain,Grains", @"Grains.ValueGrain");
        }
    }
}
#pragma warning restore 162
#pragma warning restore 219
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 649
#pragma warning restore 693
#pragma warning restore 1591
#pragma warning restore 1998
#endif
