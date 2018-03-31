﻿using Xunit;

namespace CodeConverter.Tests.CSharp
{
    public class NamespaceLevelTests : ConverterTestBase
    {
        [Fact]
        public void TestNamespace()
        {
            TestConversionVisualBasicToCSharp(@"Namespace Test
End Namespace", @"namespace Test
{
}");
        }

        [Fact]
        public void TestGlobalNamespace()
        {
            TestConversionVisualBasicToCSharp(@"Namespace Global.Test
End Namespace", @"namespace Test
{
}");
        }

        [Fact]
        public void TestTopLevelAttribute()
        {
            TestConversionVisualBasicToCSharp(
                @"<Assembly: CLSCompliant(True)>",
                @"using System;

[assembly: CLSCompliant(true)]");
        }

        [Fact]
        public void TestImports()
        {
            TestConversionVisualBasicToCSharp(
                @"Imports SomeNamespace
Imports VB = Microsoft.VisualBasic",
                @"using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;
using SomeNamespace;
using VB = Microsoft.VisualBasic;");
        }

        [Fact]
        public void TestClass()
        {
            TestConversionVisualBasicToCSharp(@"Namespace Test.[class]
    Class TestClass(Of T)
    End Class
End Namespace", @"namespace Test.@class
{
    class TestClass<T>
    {
    }
}");
        }

        [Fact]
        public void TestInternalStaticClass()
        {
            TestConversionVisualBasicToCSharp(@"Namespace Test.[class]
    Friend Module TestClass
        Sub Test()
        End Sub

        Private Sub Test2()
        End Sub
    End Module
End Namespace", @"namespace Test.@class
{
    internal static class TestClass
    {
        public static void Test()
        {
        }

        private static void Test2()
        {
        }
    }
}");
        }

        [Fact]
        public void TestAbstractClass()
        {
            TestConversionVisualBasicToCSharp(@"Namespace Test.[class]
    MustInherit Class TestClass
    End Class
End Namespace", @"namespace Test.@class
{
    abstract class TestClass
    {
    }
}");
        }

        [Fact]
        public void TestSealedClass()
        {
            TestConversionVisualBasicToCSharp(@"Namespace Test.[class]
    NotInheritable Class TestClass
    End Class
End Namespace", @"namespace Test.@class
{
    sealed class TestClass
    {
    }
}");
        }

        [Fact]
        public void TestInterface()
        {
            TestConversionVisualBasicToCSharp(
@"Interface ITest
    Inherits System.IDisposable

    Sub Test()
End Interface", @"interface ITest : System.IDisposable
{
    void Test();
}");
        }

        [Fact]
        public void TestEnum()
        {
            TestConversionVisualBasicToCSharp(
@"Friend Enum ExceptionResource
    Argument_ImplementIComparable
    ArgumentOutOfRange_NeedNonNegNum
    ArgumentOutOfRange_NeedNonNegNumRequired
    Arg_ArrayPlusOffTooSmall
End Enum", @"internal enum ExceptionResource
{
    Argument_ImplementIComparable,
    ArgumentOutOfRange_NeedNonNegNum,
    ArgumentOutOfRange_NeedNonNegNumRequired,
    Arg_ArrayPlusOffTooSmall
}");
        }

        [Fact]
        public void TestClassInheritanceList()
        {
            TestConversionVisualBasicToCSharp(
@"MustInherit Class ClassA
    Implements System.IDisposable

    Protected MustOverride Sub Test()
End Class", @"abstract class ClassA : System.IDisposable
{
    protected abstract void Test();
}");

            TestConversionVisualBasicToCSharp(
@"MustInherit Class ClassA
    Inherits System.EventArgs
    Implements System.IDisposable

    Protected MustOverride Sub Test()
End Class", @"abstract class ClassA : System.EventArgs, System.IDisposable
{
    protected abstract void Test();
}");
        }

        [Fact]
        public void TestStruct()
        {
            TestConversionVisualBasicToCSharp(
@"Structure MyType
    Implements System.IComparable(Of MyType)

    Private Sub Test()
    End Sub
End Structure", @"using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

struct MyType : System.IComparable<MyType>
{
    private void Test()
    {
    }
}");
        }

        [Fact]
        public void TestDelegate()
        {
            TestConversionVisualBasicToCSharp(
                @"Public Delegate Sub Test()",
                @"public delegate void Test();", expectUsings: false);
            TestConversionVisualBasicToCSharp(
                @"Public Delegate Function Test() As Integer",
                @"public delegate int Test();", expectUsings: false);
            TestConversionVisualBasicToCSharp(
                @"Public Delegate Sub Test(ByVal x As Integer)",
                @"public delegate void Test(int x);", expectUsings: false);
            TestConversionVisualBasicToCSharp(
                @"Public Delegate Sub Test(ByRef x As Integer)",
                @"public delegate void Test(ref int x);", expectUsings: false);
        }

        [Fact]
        public void ClassImplementsInterface()
        {
            TestConversionVisualBasicToCSharp(@"Class test
    Implements IComparable
End Class",
                @"using System;

class test : IComparable
{
}");
        }

        [Fact]
        public void ClassImplementsInterface2()
        {
            TestConversionVisualBasicToCSharp(@"Class test
    Implements System.IComparable
End Class",
                @"class test : System.IComparable
{
}");
        }

        [Fact]
        public void ClassInheritsClass()
        {
            TestConversionVisualBasicToCSharp(@"Imports System.IO

Class test
    Inherits InvalidDataException
End Class",
                @"using System.IO;

class test : InvalidDataException
{
}");
        }

        [Fact]
        public void ClassInheritsClass2()
        {
            TestConversionVisualBasicToCSharp(@"Class test
    Inherits System.IO.InvalidDataException
End Class",
                @"class test : System.IO.InvalidDataException
{
}");
        }
    }
}
