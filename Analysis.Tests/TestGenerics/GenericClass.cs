using System;
using System.Collections.Generic;
using System.Linq;

namespace AshMind.Code.Analysis.Tests.TestGenerics {
    // ReSharper disable UnusedTypeParameter
    // ReSharper disable EmptyConstructor

    public class GenericClass<T> {
        public GenericClass() {
        }
        
        public void NonGenericMethod() {
        }

        public void NonGenericMethod(int value) {
        }

        public void GenericMethod<U>() {
        }

        public void GenericMethod<U>(U value) {
        }

        public static GenericClass<T> operator+(GenericClass<T> left, GenericClass<T> right) {
            return null;
        }

        public bool NonGenericProperty { get; set; }
    }

    // ReSharper restore UnusedTypeParameter
    // ReSharper restore EmptyConstructor
}
