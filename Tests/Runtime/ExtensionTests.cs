using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using AnkleBreaker.Utils.Extensions;

namespace AnkleBreaker.Utils.Extensions.Tests
{
    public class ExtensionTests
    {
        #region String Extensions

        [Test]
        public void ToTitleCase_ConvertsUnderscoreSeparated()
        {
            // ToTitleCase capitalizes char after underscore, not the first char
            Assert.AreEqual("helloWorld", "hello_world".ToTitleCase());
        }

        [Test]
        public void ToPascalCase_ConvertsSpaceSeparated()
        {
            // Spaces become underscores then ToTitleCase: "hello world" -> "hello_world" -> "helloWorld"
            Assert.AreEqual("helloWorld", "hello world".ToPascalCase());
        }

        [Test]
        public void ToPascalCase_EmptyString_ReturnsEmpty()
        {
            Assert.AreEqual("", "".ToPascalCase());
        }

        [Test]
        public void ToCamelCase_FirstLetterLower()
        {
            string result = "hello_world".ToCamelCase(false);
            Assert.IsTrue(char.IsLower(result[0]));
        }

        [Test]
        public void ToCamelCase_FirstLetterUpper()
        {
            string result = "hello_world".ToCamelCase(true);
            Assert.IsTrue(char.IsUpper(result[0]));
        }

        [Test]
        public void Concat_ListOfStrings_JoinsWithSeparator()
        {
            var list = new List<string> { "a", "b", "c" };
            Assert.AreEqual("a, b, c", list.Concat(", "));
        }

        [Test]
        public void Concat_EmptyList_ReturnsEmpty()
        {
            var list = new List<string>();
            Assert.AreEqual("", list.Concat(", "));
        }

        [Test]
        public void Concat_SingleItem_ReturnsItem()
        {
            var list = new List<string> { "only" };
            Assert.AreEqual("only", list.Concat(", "));
        }

        [Test]
        public void IPStringToUint_ValidIP_ReturnsUint()
        {
            uint result = "192.168.1.1".IPStringToUint();
            // 192*2^24 + 168*2^16 + 1*2^8 + 1 = 3232235777
            Assert.AreEqual(3232235777u, result);
        }

        [Test]
        public void CalculateSHA256_ReturnsConsistentHash()
        {
            string hash1 = "test".CalculateSHA256HashWithNonce();
            string hash2 = "test".CalculateSHA256HashWithNonce();
            Assert.AreEqual(hash1, hash2);
            Assert.AreEqual(64, hash1.Length); // SHA256 = 64 hex chars
        }

        [Test]
        public void CalculateSHA256_DifferentNonce_DifferentHash()
        {
            string hash1 = "test".CalculateSHA256HashWithNonce("nonce1");
            string hash2 = "test".CalculateSHA256HashWithNonce("nonce2");
            Assert.AreNotEqual(hash1, hash2);
        }

        #endregion

        #region Float Extensions

        [Test]
        public void Remap_MidValue_RemapsCorrectly()
        {
            float result = 5f.Remap(0f, 10f, 0f, 100f);
            Assert.AreEqual(50f, result, 0.001f);
        }

        [Test]
        public void Remap_MinValue_ReturnsMappedMin()
        {
            float result = 0f.Remap(0f, 10f, 20f, 40f);
            Assert.AreEqual(20f, result, 0.001f);
        }

        [Test]
        public void Remap_MaxValue_ReturnsMappedMax()
        {
            float result = 10f.Remap(0f, 10f, 20f, 40f);
            Assert.AreEqual(40f, result, 0.001f);
        }

        #endregion

        #region Color Extensions

        [Test]
        public void SetAlpha_ChangesAlphaOnly()
        {
            Color original = new Color(1f, 0.5f, 0.25f, 1f);
            Color result = original.SetAlpha(0.5f);
            Assert.AreEqual(1f, result.r, 0.001f);
            Assert.AreEqual(0.5f, result.g, 0.001f);
            Assert.AreEqual(0.25f, result.b, 0.001f);
            Assert.AreEqual(0.5f, result.a, 0.001f);
        }

        #endregion

        #region Dictionary Extensions

        [Test]
        public void Find_ExistingKey_ReturnsValue()
        {
            var dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
            Assert.AreEqual(1, dict.Find("a"));
        }

        [Test]
        public void Find_MissingKey_ReturnsDefault()
        {
            var dict = new Dictionary<string, int> { { "a", 1 } };
            Assert.AreEqual(0, dict.Find("missing"));
        }

        [Test]
        public void DictionaryEqual_SameContent_ReturnsTrue()
        {
            var dict1 = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
            var dict2 = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
            Assert.IsTrue(dict1.DictionaryEqual(dict2, (v1, v2) => v1 == v2));
        }

        [Test]
        public void DictionaryEqual_DifferentContent_ReturnsFalse()
        {
            var dict1 = new Dictionary<string, int> { { "a", 1 } };
            var dict2 = new Dictionary<string, int> { { "a", 99 } };
            Assert.IsFalse(dict1.DictionaryEqual(dict2, (v1, v2) => v1 == v2));
        }

        [Test]
        public void DictionaryEqual_DifferentCount_ReturnsFalse()
        {
            var dict1 = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
            var dict2 = new Dictionary<string, int> { { "a", 1 } };
            Assert.IsFalse(dict1.DictionaryEqual(dict2, (v1, v2) => v1 == v2));
        }

        [Test]
        public void TryInvokeCallback_ExistingKey_InvokesAndRemoves()
        {
            bool invoked = false;
            var dict = new Dictionary<int, Action> { { 1, () => invoked = true } };
            bool result = dict.TryInvokeCallback(1);
            Assert.IsTrue(result);
            Assert.IsTrue(invoked);
            Assert.IsFalse(dict.ContainsKey(1));
        }

        [Test]
        public void TryInvokeCallback_MissingKey_ReturnsFalse()
        {
            var dict = new Dictionary<int, Action>();
            Assert.IsFalse(dict.TryInvokeCallback(99));
        }

        [Test]
        public void TryInvokeCallbackGeneric_PassesValue()
        {
            int received = 0;
            var dict = new Dictionary<int, Action<int>> { { 1, v => received = v } };
            dict.TryInvokeCallback(1, 42);
            Assert.AreEqual(42, received);
        }

        #endregion

        #region Enumerable Extensions

        [Test]
        public void IsNullOrEmpty_NullEnumerable_ReturnsTrue()
        {
            List<int> list = null;
            Assert.IsTrue(list.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_EmptyList_ReturnsTrue()
        {
            Assert.IsTrue(new List<int>().IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_NonEmptyList_ReturnsFalse()
        {
            Assert.IsFalse(new List<int> { 1 }.IsNullOrEmpty());
        }

        #endregion

        #region List Extensions

        [Test]
        public void IsInRange_ValidIndex_ReturnsTrue()
        {
            var list = new List<int> { 10, 20, 30 };
            Assert.IsTrue(list.IsInRange(0));
            Assert.IsTrue(list.IsInRange(2));
        }

        [Test]
        public void IsInRange_InvalidIndex_ReturnsFalse()
        {
            var list = new List<int> { 10, 20, 30 };
            Assert.IsFalse(list.IsInRange(-1));
            Assert.IsFalse(list.IsInRange(3));
        }

        #endregion

        #region FlagsHelper

        [Flags]
        private enum TestFlags
        {
            None = 0,
            A = 1,
            B = 2,
            C = 4,
            AB = A | B
        }

        [Test]
        public void FlagsHelper_IsSet_ReturnsTrue()
        {
            TestFlags flags = TestFlags.A | TestFlags.C;
            Assert.IsTrue(FlagsHelper.IsSet(flags, TestFlags.A));
            Assert.IsTrue(FlagsHelper.IsSet(flags, TestFlags.C));
        }

        [Test]
        public void FlagsHelper_IsSet_ReturnsFalse()
        {
            TestFlags flags = TestFlags.A;
            Assert.IsFalse(FlagsHelper.IsSet(flags, TestFlags.B));
        }

        [Test]
        public void FlagsHelper_IsAllSet_AllPresent_ReturnsTrue()
        {
            TestFlags flags = TestFlags.A | TestFlags.B | TestFlags.C;
            Assert.IsTrue(FlagsHelper.IsAllSet(flags, TestFlags.A, TestFlags.B));
        }

        [Test]
        public void FlagsHelper_IsAllSet_NotAllPresent_ReturnsFalse()
        {
            TestFlags flags = TestFlags.A;
            Assert.IsFalse(FlagsHelper.IsAllSet(flags, TestFlags.A, TestFlags.B));
        }

        [Test]
        public void FlagsHelper_IsAnySet_OnePresent_ReturnsTrue()
        {
            TestFlags flags = TestFlags.A;
            Assert.IsTrue(FlagsHelper.IsAnySet(flags, TestFlags.A, TestFlags.B));
        }

        [Test]
        public void FlagsHelper_IsAnySet_NonePresent_ReturnsFalse()
        {
            TestFlags flags = TestFlags.None;
            Assert.IsFalse(FlagsHelper.IsAnySet(flags, TestFlags.A, TestFlags.B));
        }

        [Test]
        public void FlagsHelper_Set_AddsFlag()
        {
            TestFlags flags = TestFlags.None;
            FlagsHelper.Set(ref flags, TestFlags.A);
            Assert.IsTrue(FlagsHelper.IsSet(flags, TestFlags.A));
        }

        [Test]
        public void FlagsHelper_Unset_RemovesFlag()
        {
            TestFlags flags = TestFlags.A | TestFlags.B;
            FlagsHelper.Unset(ref flags, TestFlags.A);
            Assert.IsFalse(FlagsHelper.IsSet(flags, TestFlags.A));
            Assert.IsTrue(FlagsHelper.IsSet(flags, TestFlags.B));
        }

        [Test]
        public void FlagsHelper_Print_ContainsFlagName()
        {
            TestFlags flags = TestFlags.A | TestFlags.B;
            string result = FlagsHelper.Print(flags);
            Assert.IsTrue(result.Contains("A"));
            Assert.IsTrue(result.Contains("B"));
        }

        [Test]
        public void FlagsHelper_IsAllSplitAreSet_AllMatched_ReturnsTrue()
        {
            TestFlags flags1 = TestFlags.A | TestFlags.B;
            TestFlags flags2 = TestFlags.A | TestFlags.B | TestFlags.C;
            Assert.IsTrue(FlagsHelper.IsAllSplitAreSet(flags1, flags2));
        }

        [Test]
        public void FlagsHelper_IsAllSplitAreSet_NotAllMatched_ReturnsFalse()
        {
            TestFlags flags1 = TestFlags.A | TestFlags.B;
            TestFlags flags2 = TestFlags.A;
            Assert.IsFalse(FlagsHelper.IsAllSplitAreSet(flags1, flags2));
        }

        #endregion

        #region PositionRotation

        [Test]
        public void PositionRotation_IsSerializable()
        {
            var pr = new PositionRotation
            {
                Position = new Vector3(1, 2, 3),
                Rotation = Quaternion.identity
            };
            Assert.AreEqual(new Vector3(1, 2, 3), pr.Position);
            Assert.AreEqual(Quaternion.identity, pr.Rotation);
        }

        #endregion

        #region PositionRotation Extensions

        [Test]
        public void InverseTransformPointEquivalent_IdentityRotation_SubtractsPosition()
        {
            var pr = new PositionRotation
            {
                Position = new Vector3(10, 0, 0),
                Rotation = Quaternion.identity
            };
            Vector3 result = pr.InverseTransformPointEquivalent(new Vector3(15, 0, 0));
            Assert.AreEqual(5f, result.x, 0.001f);
            Assert.AreEqual(0f, result.y, 0.001f);
            Assert.AreEqual(0f, result.z, 0.001f);
        }

        [Test]
        public void TransformPointEquivalent_IdentityRotation_AddsPosition()
        {
            var pr = new PositionRotation
            {
                Position = new Vector3(10, 0, 0),
                Rotation = Quaternion.identity
            };
            Vector3 result = pr.TransformPointEquivalent(new Vector3(5, 0, 0));
            Assert.AreEqual(15f, result.x, 0.001f);
        }

        [Test]
        public void InverseTransform_ThenTransform_ReturnsOriginal()
        {
            var pr = new PositionRotation
            {
                Position = new Vector3(3, 5, 7),
                Rotation = Quaternion.Euler(30, 45, 60)
            };
            Vector3 worldPoint = new Vector3(10, 20, 30);
            Vector3 local = pr.InverseTransformPointEquivalent(worldPoint);
            Vector3 backToWorld = pr.TransformPointEquivalent(local);
            Assert.AreEqual(worldPoint.x, backToWorld.x, 0.01f);
            Assert.AreEqual(worldPoint.y, backToWorld.y, 0.01f);
            Assert.AreEqual(worldPoint.z, backToWorld.z, 0.01f);
        }

        [Test]
        public void FromTransform_CopiesPositionAndRotation()
        {
            var go = new GameObject("TestPR");
            go.transform.position = new Vector3(1, 2, 3);
            go.transform.rotation = Quaternion.Euler(10, 20, 30);

            var pr = PositionRotationExtensions.FromTransform(go.transform);
            Assert.AreEqual(go.transform.position.x, pr.Position.x, 0.001f);
            Assert.AreEqual(go.transform.position.y, pr.Position.y, 0.001f);
            Assert.AreEqual(go.transform.position.z, pr.Position.z, 0.001f);

            UnityEngine.Object.DestroyImmediate(go);
        }

        #endregion

        #region Vector Extensions

        [Test]
        public void Vector3_With_OverridesSpecifiedComponents()
        {
            Vector3 v = new Vector3(1f, 2f, 3f);
            Vector3 result = v.With(y: 0f);
            Assert.AreEqual(1f, result.x, 0.001f);
            Assert.AreEqual(0f, result.y, 0.001f);
            Assert.AreEqual(3f, result.z, 0.001f);
        }

        [Test]
        public void Vector3_With_MultipleOverrides()
        {
            Vector3 v = new Vector3(1f, 2f, 3f);
            Vector3 result = v.With(x: 10f, z: 30f);
            Assert.AreEqual(10f, result.x, 0.001f);
            Assert.AreEqual(2f, result.y, 0.001f);
            Assert.AreEqual(30f, result.z, 0.001f);
        }

        [Test]
        public void Vector3_Flat_ZerosOutY()
        {
            Vector3 v = new Vector3(5f, 99f, 10f);
            Vector3 result = v.Flat();
            Assert.AreEqual(5f, result.x, 0.001f);
            Assert.AreEqual(0f, result.y, 0.001f);
            Assert.AreEqual(10f, result.z, 0.001f);
        }

        [Test]
        public void Vector2_ToVector3XZ_ConvertsCorrectly()
        {
            Vector2 v = new Vector2(3f, 7f);
            Vector3 result = v.ToVector3XZ();
            Assert.AreEqual(3f, result.x, 0.001f);
            Assert.AreEqual(0f, result.y, 0.001f);
            Assert.AreEqual(7f, result.z, 0.001f);
        }

        [Test]
        public void Vector2_With_OverridesSpecifiedComponent()
        {
            Vector2 v = new Vector2(1f, 2f);
            Vector2 result = v.With(y: 5f);
            Assert.AreEqual(1f, result.x, 0.001f);
            Assert.AreEqual(5f, result.y, 0.001f);
        }

        #endregion

        #region List Extensions - Shuffle & GetRandom

        [Test]
        public void List_GetRandom_ReturnsElementFromList()
        {
            var list = new List<int> { 10, 20, 30 };
            int result = list.GetRandom();
            Assert.IsTrue(list.Contains(result));
        }

        [Test]
        public void List_GetRandom_ThrowsOnEmptyList()
        {
            var list = new List<int>();
            Assert.Throws<InvalidOperationException>(() => list.GetRandom());
        }

        [Test]
        public void List_Shuffle_PreservesElements()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            var original = new List<int>(list);
            list.Shuffle();
            Assert.AreEqual(original.Count, list.Count);
            foreach (int item in original)
                Assert.IsTrue(list.Contains(item));
        }

        #endregion

        #region Enumerable Extensions - ForEach

        [Test]
        public void ForEach_ExecutesActionOnEachElement()
        {
            var items = new List<int> { 1, 2, 3 };
            int sum = 0;
            ((IEnumerable<int>)items).ForEach(x => sum += x);
            Assert.AreEqual(6, sum);
        }

        [Test]
        public void ForEach_NullEnumerable_DoesNotThrow()
        {
            IEnumerable<int> items = null;
            Assert.DoesNotThrow(() => items.ForEach(x => { }));
        }

        #endregion

        #region Color Extensions - WithR/G/B & ToHexString

        [Test]
        public void Color_WithR_ChangesOnlyR()
        {
            Color c = new Color(0.1f, 0.2f, 0.3f, 0.4f);
            Color result = c.WithR(0.9f);
            Assert.AreEqual(0.9f, result.r, 0.001f);
            Assert.AreEqual(0.2f, result.g, 0.001f);
            Assert.AreEqual(0.3f, result.b, 0.001f);
            Assert.AreEqual(0.4f, result.a, 0.001f);
        }

        [Test]
        public void Color_WithG_ChangesOnlyG()
        {
            Color c = new Color(0.1f, 0.2f, 0.3f, 0.4f);
            Color result = c.WithG(0.8f);
            Assert.AreEqual(0.1f, result.r, 0.001f);
            Assert.AreEqual(0.8f, result.g, 0.001f);
        }

        [Test]
        public void Color_WithB_ChangesOnlyB()
        {
            Color c = new Color(0.1f, 0.2f, 0.3f, 0.4f);
            Color result = c.WithB(0.7f);
            Assert.AreEqual(0.7f, result.b, 0.001f);
            Assert.AreEqual(0.4f, result.a, 0.001f);
        }

        [Test]
        public void Color_ToHexString_WithoutAlpha()
        {
            Color c = new Color(1f, 0f, 0f, 1f);
            Assert.AreEqual("#FF0000", c.ToHexString());
        }

        [Test]
        public void Color_ToHexString_WithAlpha()
        {
            Color c = new Color(1f, 0f, 0f, 0.5f);
            string hex = c.ToHexString(true);
            Assert.IsTrue(hex.StartsWith("#FF0000"));
            Assert.AreEqual(9, hex.Length); // #RRGGBBAA
        }

        #endregion

        #region LayerMask Extensions

        [Test]
        public void LayerMask_AddLayer_AddsCorrectBit()
        {
            LayerMask mask = 0;
            mask = mask.AddLayer(5);
            Assert.IsTrue(mask.Contains(5));
        }

        [Test]
        public void LayerMask_Contains_ReturnsFalseForMissingLayer()
        {
            LayerMask mask = 1 << 3;
            Assert.IsFalse(mask.Contains(5));
        }

        #endregion

        #region AB_Random

        [Test]
        public void ABRandom_Range_ExcludesSpecifiedNumber()
        {
            // With range 0..2 excluding 0, the only option is 1
            int result = AB_Random.Range(0, 2, 0);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void ABRandom_Range_ThrowsWhenOnlyExcluded()
        {
            Assert.Throws<Exception>(() => AB_Random.Range(5, 6, 5));
        }

        #endregion
    }
}
