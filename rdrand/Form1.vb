Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices


Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        For i As Integer = 0 To 99 Step 1
            Debug.WriteLine(RDRAND.Rand())
        Next

    End Sub
End Class


''' <summary>
''' Source https://i255.wordpress.com/2015/12/08/calling-rdrand-and-rdseed-from-netc/
''' </summary>
NotInheritable Class RDRAND
    Private Sub New()
    End Sub
    Shared Sub New()
        ' rdrand eax
        ' setc byte ptr [rcx]
        Dim codeRand = New Byte() {&HF, &HC7, &HF0, &HF, &H92, &H1,
            &HC3}
        ' ret
        ' rdseed eax
        '0x67, 0x0f, 0x92, 0x01, // setc byte ptr [ecx]
        ' setc byte ptr [rcx]
        Dim codeSeed = New Byte() {&HF, &HC7, &HF8, &HF, &H92, &H1,
            &HC3}
        ' ret
        Dim status As Byte
        SeedNative(status)
        RandNative(status)

        Marshal.Copy(codeSeed, 0, GetType(RDRAND).GetMethod(NameOf(SeedNative)).MethodHandle.GetFunctionPointer(), codeSeed.Length)
        Marshal.Copy(codeRand, 0, GetType(RDRAND).GetMethod(NameOf(RandNative)).MethodHandle.GetFunctionPointer(), codeRand.Length)
    End Sub

    Public Shared Function Seed() As UInteger
        Dim res As UInteger
        Dim status As Byte
        Do
            res = SeedNative(status)
        Loop While status = 0
        Return res
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Public Shared Function SeedNative(ByRef status As Byte) As UInteger
        status = 16
        Return 32
    End Function

    Public Shared Function Rand() As UInteger
        Dim res As UInteger
        Dim status As Byte
        Do
            res = RandNative(status)
        Loop While status = 0
        Return res
    End Function

    <MethodImpl(MethodImplOptions.NoInlining)>
    Public Shared Function RandNative(ByRef status As Byte) As UInteger
        status = 16
        Return 32
    End Function
End Class
