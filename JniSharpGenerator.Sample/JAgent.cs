using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JavaNETInterface.Jni;
using JavaNETInterface.Jvmti;
using java.io;
using java.util;

namespace JniSharpGenerator.Sample;

public static unsafe class JAgent
{
    static JvmtiEventCallbacks callbacks = default;
    [UnmanagedCallersOnly(EntryPoint = "Agent_OnLoad")]
    public static int Agent_OnLoad(JavaVM* vm, byte* options, void* reserved)
        => Initialize(vm, options);

    [UnmanagedCallersOnly(EntryPoint = "Agent_OnAttach")]
    public static int Agent_OnAttach(JavaVM* vm, byte* options, void* reserved)
        => Initialize(vm, options);

    [UnmanagedCallersOnly(EntryPoint = "Agent_OnUnload")]
    public static void Agent_OnUnload(JavaVM* vm) { }

    private static int Initialize(JavaVM* vm, byte* options)
    {
        JvmtiEnv* jvmti = null;
        int res = vm->GetEnv((void**)&jvmti, JvmtiConstants.JVMTI_VERSION);
        if (res != JniConstants.JNI_OK || jvmti == null)
            return -1;

        callbacks.VMInit = &OnVMInit;

        fixed(JvmtiEventCallbacks* cptr = &callbacks)
            jvmti->SetEventCallbacks(cptr, sizeof(JvmtiEventCallbacks));

        jvmti->SetEventNotificationMode(JvmtiEventMode.ENABLE, JvmtiEvent.VM_INIT, null);

        return 0;
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    public static void OnVMInit(JvmtiEnv* jvmtiEnv, JniEnv* jniEnv, JObject* thread)
    {
        java.lang.System.Initialize(jniEnv);
        java.lang.System.GetOut(jniEnv).Println("[NativeAOT] JVM initialized.");
    }

    [UnmanagedCallersOnly(EntryPoint = "Java_JniSharpGenerator_Sample_JavaSample_nativeSharpMethod", CallConvs = [typeof(CallConvCdecl)])]
    public static void NativeSharpMethod(JniEnv* env, JClass* klass)
    {
        JavaSample.Initialize(env);

        PrintStream outStream = java.lang.System.GetOut(env);

        outStream.Print("[NativeAOT] Enter an integer: ");
        
        Scanner scanner = Scanner.Scanner2_Constructor(env, java.lang.System.GetIn(env));
        
        int param = scanner.NextInt();

        outStream.Println($"[NativeAOT] Result of {JavaSample.GetValue(env)} + {param} is {JavaSample.Add(env, JavaSample.GetValue(env), param)}");

        scanner.Close();
    }
}