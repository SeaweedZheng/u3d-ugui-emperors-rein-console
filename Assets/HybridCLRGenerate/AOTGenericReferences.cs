using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"Newtonsoft.Json.dll",
		"System.Core.dll",
		"System.dll",
		"UnityEngine.AndroidJNIModule.dll",
		"UnityEngine.AssetBundleModule.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// System.Action<Loom.DelayedQueueItem>
	// System.Action<Loom.NoDelayedQueueItem>
	// System.Action<Spine.EventQueue.EventQueueEntry>
	// System.Action<Spine.Skin.SkinEntry>
	// System.Action<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Action<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Action<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Action<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Action<Spine.Unity.SubmeshInstruction>
	// System.Action<System.Guid>
	// System.Action<UnityEngine.AnimatorClipInfo>
	// System.Action<UnityEngine.Color32>
	// System.Action<UnityEngine.UIVertex>
	// System.Action<UnityEngine.Vector2>
	// System.Action<UnityEngine.Vector3>
	// System.Action<byte,object>
	// System.Action<byte>
	// System.Action<float>
	// System.Action<int>
	// System.Action<long>
	// System.Action<object,object>
	// System.Action<object>
	// System.Collections.Concurrent.ConcurrentDictionary.<GetEnumerator>d__32<System.Guid,object>
	// System.Collections.Concurrent.ConcurrentDictionary.DictionaryEnumerator<System.Guid,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Node<System.Guid,object>
	// System.Collections.Concurrent.ConcurrentDictionary.Tables<System.Guid,object>
	// System.Collections.Concurrent.ConcurrentDictionary<System.Guid,object>
	// System.Collections.Generic.ArraySortHelper<Loom.DelayedQueueItem>
	// System.Collections.Generic.ArraySortHelper<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.ArraySortHelper<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.ArraySortHelper<Spine.Skin.SkinEntry>
	// System.Collections.Generic.ArraySortHelper<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.ArraySortHelper<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.ArraySortHelper<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.ArraySortHelper<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.ArraySortHelper<System.Guid>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Color32>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.UIVertex>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector2>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3>
	// System.Collections.Generic.ArraySortHelper<byte>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<long>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.Comparer<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.Comparer<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.Comparer<Spine.Skin.SkinEntry>
	// System.Collections.Generic.Comparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.Comparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.Comparer<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.Comparer<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.Comparer<Spine.Unity.SubmeshInstruction>
	// System.Collections.Generic.Comparer<System.Guid>
	// System.Collections.Generic.Comparer<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.Comparer<UnityEngine.Color32>
	// System.Collections.Generic.Comparer<UnityEngine.UIVertex>
	// System.Collections.Generic.Comparer<UnityEngine.Vector2>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<float>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<long>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.ComparisonComparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.ComparisonComparer<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.ComparisonComparer<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.ComparisonComparer<Spine.Skin.SkinEntry>
	// System.Collections.Generic.ComparisonComparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.ComparisonComparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.ComparisonComparer<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.ComparisonComparer<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.ComparisonComparer<Spine.Unity.SubmeshInstruction>
	// System.Collections.Generic.ComparisonComparer<System.Guid>
	// System.Collections.Generic.ComparisonComparer<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.ComparisonComparer<UnityEngine.Color32>
	// System.Collections.Generic.ComparisonComparer<UnityEngine.UIVertex>
	// System.Collections.Generic.ComparisonComparer<UnityEngine.Vector2>
	// System.Collections.Generic.ComparisonComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ComparisonComparer<byte>
	// System.Collections.Generic.ComparisonComparer<float>
	// System.Collections.Generic.ComparisonComparer<int>
	// System.Collections.Generic.ComparisonComparer<long>
	// System.Collections.Generic.ComparisonComparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<Spine.AnimationStateData.AnimationPair,float>
	// System.Collections.Generic.Dictionary.Enumerator<Spine.Skin.SkinKey,Spine.Skin.SkinEntry>
	// System.Collections.Generic.Dictionary.Enumerator<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey,object>
	// System.Collections.Generic.Dictionary.Enumerator<System.Collections.Generic.KeyValuePair<object,object>,object>
	// System.Collections.Generic.Dictionary.Enumerator<int,byte>
	// System.Collections.Generic.Dictionary.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<Spine.AnimationStateData.AnimationPair,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<Spine.Skin.SkinKey,Spine.Skin.SkinEntry>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<System.Collections.Generic.KeyValuePair<object,object>,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.KeyCollection<Spine.AnimationStateData.AnimationPair,float>
	// System.Collections.Generic.Dictionary.KeyCollection<Spine.Skin.SkinKey,Spine.Skin.SkinEntry>
	// System.Collections.Generic.Dictionary.KeyCollection<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey,object>
	// System.Collections.Generic.Dictionary.KeyCollection<System.Collections.Generic.KeyValuePair<object,object>,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<ulong,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<Spine.AnimationStateData.AnimationPair,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<Spine.Skin.SkinKey,Spine.Skin.SkinEntry>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<System.Collections.Generic.KeyValuePair<object,object>,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.ValueCollection<Spine.AnimationStateData.AnimationPair,float>
	// System.Collections.Generic.Dictionary.ValueCollection<Spine.Skin.SkinKey,Spine.Skin.SkinEntry>
	// System.Collections.Generic.Dictionary.ValueCollection<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey,object>
	// System.Collections.Generic.Dictionary.ValueCollection<System.Collections.Generic.KeyValuePair<object,object>,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<ulong,object>
	// System.Collections.Generic.Dictionary<Spine.AnimationStateData.AnimationPair,float>
	// System.Collections.Generic.Dictionary<Spine.Skin.SkinKey,Spine.Skin.SkinEntry>
	// System.Collections.Generic.Dictionary<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey,object>
	// System.Collections.Generic.Dictionary<System.Collections.Generic.KeyValuePair<object,object>,object>
	// System.Collections.Generic.Dictionary<int,byte>
	// System.Collections.Generic.Dictionary<int,int>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<long,object>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<ulong,object>
	// System.Collections.Generic.EqualityComparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.EqualityComparer<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.EqualityComparer<Spine.AnimationStateData.AnimationPair>
	// System.Collections.Generic.EqualityComparer<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.EqualityComparer<Spine.Skin.SkinEntry>
	// System.Collections.Generic.EqualityComparer<Spine.Skin.SkinKey>
	// System.Collections.Generic.EqualityComparer<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey>
	// System.Collections.Generic.EqualityComparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.EqualityComparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.EqualityComparer<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.EqualityComparer<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.EqualityComparer<System.Guid>
	// System.Collections.Generic.EqualityComparer<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Color32>
	// System.Collections.Generic.EqualityComparer<UnityEngine.UIVertex>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Vector2>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Vector3>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<long>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.EqualityComparer<ulong>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<Loom.DelayedQueueItem>
	// System.Collections.Generic.ICollection<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.ICollection<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.ICollection<Spine.Skin.SkinEntry>
	// System.Collections.Generic.ICollection<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.ICollection<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.ICollection<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.ICollection<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.ICollection<Spine.Unity.SubmeshInstruction>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Spine.AnimationStateData.AnimationPair,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Spine.Skin.SkinKey,Spine.Skin.SkinEntry>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Collections.Generic.KeyValuePair<object,object>,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.ICollection<System.Guid>
	// System.Collections.Generic.ICollection<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.ICollection<UnityEngine.Color32>
	// System.Collections.Generic.ICollection<UnityEngine.UIVertex>
	// System.Collections.Generic.ICollection<UnityEngine.Vector2>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3>
	// System.Collections.Generic.ICollection<byte>
	// System.Collections.Generic.ICollection<float>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<long>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.IComparer<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.IComparer<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.IComparer<Spine.Skin.SkinEntry>
	// System.Collections.Generic.IComparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.IComparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.IComparer<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.IComparer<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.IComparer<System.Guid>
	// System.Collections.Generic.IComparer<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.IComparer<UnityEngine.Color32>
	// System.Collections.Generic.IComparer<UnityEngine.UIVertex>
	// System.Collections.Generic.IComparer<UnityEngine.Vector2>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3>
	// System.Collections.Generic.IComparer<byte>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<long>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IDictionary<System.Guid,object>
	// System.Collections.Generic.IEnumerable<Loom.DelayedQueueItem>
	// System.Collections.Generic.IEnumerable<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.IEnumerable<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.IEnumerable<Spine.Skin.SkinEntry>
	// System.Collections.Generic.IEnumerable<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.IEnumerable<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.IEnumerable<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.IEnumerable<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.IEnumerable<Spine.Unity.SubmeshInstruction>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<Spine.AnimationStateData.AnimationPair,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<Spine.Skin.SkinKey,Spine.Skin.SkinEntry>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Collections.Generic.KeyValuePair<object,object>,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Guid,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IEnumerable<System.Guid>
	// System.Collections.Generic.IEnumerable<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.IEnumerable<UnityEngine.Color32>
	// System.Collections.Generic.IEnumerable<UnityEngine.UIVertex>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector2>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerable<byte>
	// System.Collections.Generic.IEnumerable<float>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<long>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<Loom.DelayedQueueItem>
	// System.Collections.Generic.IEnumerator<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.IEnumerator<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.IEnumerator<Spine.Skin.SkinEntry>
	// System.Collections.Generic.IEnumerator<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.IEnumerator<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.IEnumerator<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.IEnumerator<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.IEnumerator<Spine.Unity.SubmeshInstruction>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<Spine.AnimationStateData.AnimationPair,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<Spine.Skin.SkinKey,Spine.Skin.SkinEntry>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Collections.Generic.KeyValuePair<object,object>,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Guid,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IEnumerator<System.Guid>
	// System.Collections.Generic.IEnumerator<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.IEnumerator<UnityEngine.Color32>
	// System.Collections.Generic.IEnumerator<UnityEngine.UIVertex>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector2>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerator<byte>
	// System.Collections.Generic.IEnumerator<float>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<long>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<Spine.AnimationStateData.AnimationPair>
	// System.Collections.Generic.IEqualityComparer<Spine.Skin.SkinKey>
	// System.Collections.Generic.IEqualityComparer<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey>
	// System.Collections.Generic.IEqualityComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEqualityComparer<System.Guid>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<long>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IEqualityComparer<ulong>
	// System.Collections.Generic.IList<Loom.DelayedQueueItem>
	// System.Collections.Generic.IList<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.IList<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.IList<Spine.Skin.SkinEntry>
	// System.Collections.Generic.IList<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.IList<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.IList<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.IList<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.IList<System.Guid>
	// System.Collections.Generic.IList<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.IList<UnityEngine.Color32>
	// System.Collections.Generic.IList<UnityEngine.UIVertex>
	// System.Collections.Generic.IList<UnityEngine.Vector2>
	// System.Collections.Generic.IList<UnityEngine.Vector3>
	// System.Collections.Generic.IList<byte>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<long>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<Spine.AnimationStateData.AnimationPair,float>
	// System.Collections.Generic.KeyValuePair<Spine.Skin.SkinKey,Spine.Skin.SkinEntry>
	// System.Collections.Generic.KeyValuePair<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey,object>
	// System.Collections.Generic.KeyValuePair<System.Collections.Generic.KeyValuePair<object,object>,object>
	// System.Collections.Generic.KeyValuePair<System.Guid,object>
	// System.Collections.Generic.KeyValuePair<int,byte>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<long,object>
	// System.Collections.Generic.KeyValuePair<object,float>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<ulong,object>
	// System.Collections.Generic.LinkedList.Enumerator<object>
	// System.Collections.Generic.LinkedList<object>
	// System.Collections.Generic.LinkedListNode<object>
	// System.Collections.Generic.List.Enumerator<Loom.DelayedQueueItem>
	// System.Collections.Generic.List.Enumerator<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.List.Enumerator<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.List.Enumerator<Spine.Skin.SkinEntry>
	// System.Collections.Generic.List.Enumerator<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.List.Enumerator<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.List.Enumerator<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.List.Enumerator<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.List.Enumerator<System.Guid>
	// System.Collections.Generic.List.Enumerator<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Color32>
	// System.Collections.Generic.List.Enumerator<UnityEngine.UIVertex>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector2>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector3>
	// System.Collections.Generic.List.Enumerator<byte>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<long>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List.SynchronizedList<Loom.DelayedQueueItem>
	// System.Collections.Generic.List.SynchronizedList<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.List.SynchronizedList<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.List.SynchronizedList<Spine.Skin.SkinEntry>
	// System.Collections.Generic.List.SynchronizedList<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.List.SynchronizedList<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.List.SynchronizedList<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.List.SynchronizedList<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.List.SynchronizedList<System.Guid>
	// System.Collections.Generic.List.SynchronizedList<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.List.SynchronizedList<UnityEngine.Color32>
	// System.Collections.Generic.List.SynchronizedList<UnityEngine.UIVertex>
	// System.Collections.Generic.List.SynchronizedList<UnityEngine.Vector2>
	// System.Collections.Generic.List.SynchronizedList<UnityEngine.Vector3>
	// System.Collections.Generic.List.SynchronizedList<byte>
	// System.Collections.Generic.List.SynchronizedList<int>
	// System.Collections.Generic.List.SynchronizedList<long>
	// System.Collections.Generic.List.SynchronizedList<object>
	// System.Collections.Generic.List<Loom.DelayedQueueItem>
	// System.Collections.Generic.List<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.List<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.List<Spine.Skin.SkinEntry>
	// System.Collections.Generic.List<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.List<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.List<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.List<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.List<System.Guid>
	// System.Collections.Generic.List<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.List<UnityEngine.Color32>
	// System.Collections.Generic.List<UnityEngine.UIVertex>
	// System.Collections.Generic.List<UnityEngine.Vector2>
	// System.Collections.Generic.List<UnityEngine.Vector3>
	// System.Collections.Generic.List<byte>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<long>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.ObjectComparer<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.ObjectComparer<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.ObjectComparer<Spine.Skin.SkinEntry>
	// System.Collections.Generic.ObjectComparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.ObjectComparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.ObjectComparer<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.ObjectComparer<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.ObjectComparer<Spine.Unity.SubmeshInstruction>
	// System.Collections.Generic.ObjectComparer<System.Guid>
	// System.Collections.Generic.ObjectComparer<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Color32>
	// System.Collections.Generic.ObjectComparer<UnityEngine.UIVertex>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector2>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<float>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<long>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<Loom.DelayedQueueItem>
	// System.Collections.Generic.ObjectEqualityComparer<Loom.NoDelayedQueueItem>
	// System.Collections.Generic.ObjectEqualityComparer<Spine.AnimationStateData.AnimationPair>
	// System.Collections.Generic.ObjectEqualityComparer<Spine.EventQueue.EventQueueEntry>
	// System.Collections.Generic.ObjectEqualityComparer<Spine.Skin.SkinEntry>
	// System.Collections.Generic.ObjectEqualityComparer<Spine.Skin.SkinKey>
	// System.Collections.Generic.ObjectEqualityComparer<Spine.Unity.AttachmentTools.AtlasUtilities.IntAndAtlasRegionKey>
	// System.Collections.Generic.ObjectEqualityComparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.ObjectEqualityComparer<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.Generic.ObjectEqualityComparer<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.Generic.ObjectEqualityComparer<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ObjectEqualityComparer<System.Guid>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.AnimatorClipInfo>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Color32>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.UIVertex>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Vector2>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<ulong>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<Loom.DelayedQueueItem>
	// System.Collections.ObjectModel.ReadOnlyCollection<Loom.NoDelayedQueueItem>
	// System.Collections.ObjectModel.ReadOnlyCollection<Spine.EventQueue.EventQueueEntry>
	// System.Collections.ObjectModel.ReadOnlyCollection<Spine.Skin.SkinEntry>
	// System.Collections.ObjectModel.ReadOnlyCollection<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Collections.ObjectModel.ReadOnlyCollection<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Collections.ObjectModel.ReadOnlyCollection<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Collections.ObjectModel.ReadOnlyCollection<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Guid>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.AnimatorClipInfo>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Color32>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.UIVertex>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector2>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3>
	// System.Collections.ObjectModel.ReadOnlyCollection<byte>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<long>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<Loom.DelayedQueueItem>
	// System.Comparison<Loom.NoDelayedQueueItem>
	// System.Comparison<Spine.EventQueue.EventQueueEntry>
	// System.Comparison<Spine.Skin.SkinEntry>
	// System.Comparison<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Comparison<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Comparison<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Comparison<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Comparison<Spine.Unity.SubmeshInstruction>
	// System.Comparison<System.Guid>
	// System.Comparison<UnityEngine.AnimatorClipInfo>
	// System.Comparison<UnityEngine.Color32>
	// System.Comparison<UnityEngine.UIVertex>
	// System.Comparison<UnityEngine.Vector2>
	// System.Comparison<UnityEngine.Vector3>
	// System.Comparison<byte>
	// System.Comparison<float>
	// System.Comparison<int>
	// System.Comparison<long>
	// System.Comparison<object>
	// System.Converter<object,object>
	// System.EventHandler<object>
	// System.Func<Loom.DelayedQueueItem,byte>
	// System.Func<System.Guid,object,object>
	// System.Func<System.Guid,object>
	// System.Func<UnityEngine.Color,UnityEngine.Color,float,UnityEngine.Color>
	// System.Func<UnityEngine.Vector3,UnityEngine.Vector3,float,UnityEngine.Vector3>
	// System.Func<byte>
	// System.Func<object,byte>
	// System.Func<object,object>
	// System.Func<object>
	// System.IEquatable<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.IEquatable<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.IEquatable<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.IEquatable<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Linq.Buffer<byte>
	// System.Linq.Enumerable.<CastIterator>d__99<int>
	// System.Linq.Enumerable.<TakeIterator>d__25<byte>
	// System.Linq.Enumerable.Iterator<Loom.DelayedQueueItem>
	// System.Linq.Enumerable.WhereArrayIterator<Loom.DelayedQueueItem>
	// System.Linq.Enumerable.WhereEnumerableIterator<Loom.DelayedQueueItem>
	// System.Linq.Enumerable.WhereListIterator<Loom.DelayedQueueItem>
	// System.Nullable<float>
	// System.Nullable<int>
	// System.Predicate<Loom.DelayedQueueItem>
	// System.Predicate<Loom.NoDelayedQueueItem>
	// System.Predicate<Spine.EventQueue.EventQueueEntry>
	// System.Predicate<Spine.Skin.SkinEntry>
	// System.Predicate<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride>
	// System.Predicate<Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride>
	// System.Predicate<Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride>
	// System.Predicate<Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride>
	// System.Predicate<Spine.Unity.SubmeshInstruction>
	// System.Predicate<System.Guid>
	// System.Predicate<UnityEngine.AnimatorClipInfo>
	// System.Predicate<UnityEngine.Color32>
	// System.Predicate<UnityEngine.UIVertex>
	// System.Predicate<UnityEngine.Vector2>
	// System.Predicate<UnityEngine.Vector3>
	// System.Predicate<byte>
	// System.Predicate<float>
	// System.Predicate<int>
	// System.Predicate<long>
	// System.Predicate<object>
	// UnityEngine.Events.InvokableCall<byte>
	// UnityEngine.Events.UnityAction<System.DateTime>
	// UnityEngine.Events.UnityAction<byte>
	// UnityEngine.Events.UnityAction<float>
	// UnityEngine.Events.UnityAction<int>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityAction<ulong>
	// UnityEngine.Events.UnityEvent<byte>
	// }}

	public void RefMethods()
	{
		// object Newtonsoft.Json.JsonConvert.DeserializeObject<object>(string)
		// object Newtonsoft.Json.JsonConvert.DeserializeObject<object>(string,Newtonsoft.Json.JsonSerializerSettings)
		// object System.Activator.CreateInstance<object>()
		// int System.Array.BinarySearch<object>(object[],int,int,object)
		// int System.Array.BinarySearch<object>(object[],int,int,object,System.Collections.Generic.IComparer<object>)
		// object[] System.Array.Empty<object>()
		// int System.Array.IndexOf<object>(object[],object,int,int)
		// int System.Array.IndexOfImpl<object>(object[],object,int,int)
		// int System.Array.LastIndexOf<object>(object[],object,int,int)
		// int System.Array.LastIndexOfImpl<object>(object[],object,int,int)
		// System.Void System.Array.Resize<UnityEngine.Color32>(UnityEngine.Color32[]&,int)
		// System.Void System.Array.Resize<UnityEngine.Vector2>(UnityEngine.Vector2[]&,int)
		// System.Void System.Array.Resize<UnityEngine.Vector3>(UnityEngine.Vector3[]&,int)
		// System.Void System.Array.Resize<UnityEngine.Vector4>(UnityEngine.Vector4[]&,int)
		// System.Void System.Array.Resize<int>(int[]&,int)
		// System.Void System.Array.Resize<object>(object[]&,int)
		// System.Void System.Array.Sort<object>(object[],System.Comparison<object>)
		// System.Void System.Array.Sort<object>(object[],int,int,System.Collections.Generic.IComparer<object>)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Cast<int>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.CastIterator<int>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.Take<byte>(System.Collections.Generic.IEnumerable<byte>,int)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.TakeIterator<byte>(System.Collections.Generic.IEnumerable<byte>,int)
		// byte[] System.Linq.Enumerable.ToArray<byte>(System.Collections.Generic.IEnumerable<byte>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<Loom.DelayedQueueItem> System.Linq.Enumerable.Where<Loom.DelayedQueueItem>(System.Collections.Generic.IEnumerable<Loom.DelayedQueueItem>,System.Func<Loom.DelayedQueueItem,bool>)
		// byte UnityEngine.AndroidJNIHelper.ConvertFromJNIArray<byte>(System.IntPtr)
		// object UnityEngine.AndroidJNIHelper.ConvertFromJNIArray<object>(System.IntPtr)
		// System.IntPtr UnityEngine.AndroidJNIHelper.GetFieldID<object>(System.IntPtr,string,bool)
		// System.IntPtr UnityEngine.AndroidJNIHelper.GetMethodID<byte>(System.IntPtr,string,object[],bool)
		// System.IntPtr UnityEngine.AndroidJNIHelper.GetMethodID<object>(System.IntPtr,string,object[],bool)
		// byte UnityEngine.AndroidJavaObject.Call<byte>(string,object[])
		// object UnityEngine.AndroidJavaObject.Call<object>(string,object[])
		// object UnityEngine.AndroidJavaObject.GetStatic<object>(string)
		// byte UnityEngine.AndroidJavaObject._Call<byte>(string,object[])
		// object UnityEngine.AndroidJavaObject._Call<object>(string,object[])
		// object UnityEngine.AndroidJavaObject._GetStatic<object>(string)
		// object UnityEngine.AssetBundle.LoadAsset<object>(string)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>()
		// object UnityEngine.Component.GetComponentInParent<object>()
		// object[] UnityEngine.Component.GetComponents<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// bool UnityEngine.Component.TryGetComponent<object>(object&)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object[] UnityEngine.GameObject.GetComponents<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// bool UnityEngine.GameObject.TryGetComponent<object>(object&)
		// object UnityEngine.Object.FindObjectOfType<object>()
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object[] UnityEngine.Resources.ConvertObjects<object>(UnityEngine.Object[])
		// object[] UnityEngine.Resources.FindObjectsOfTypeAll<object>()
		// object UnityEngine.Resources.GetBuiltinResource<object>(string)
		// object UnityEngine.Resources.Load<object>(string)
		// UnityEngine.ResourceRequest UnityEngine.Resources.LoadAsync<object>(string)
		// object UnityEngine.ScriptableObject.CreateInstance<object>()
		// byte UnityEngine._AndroidJNIHelper.ConvertFromJNIArray<byte>(System.IntPtr)
		// object UnityEngine._AndroidJNIHelper.ConvertFromJNIArray<object>(System.IntPtr)
		// System.IntPtr UnityEngine._AndroidJNIHelper.GetFieldID<object>(System.IntPtr,string,bool)
		// System.IntPtr UnityEngine._AndroidJNIHelper.GetMethodID<byte>(System.IntPtr,string,object[],bool)
		// System.IntPtr UnityEngine._AndroidJNIHelper.GetMethodID<object>(System.IntPtr,string,object[],bool)
		// string UnityEngine._AndroidJNIHelper.GetSignature<byte>(object[])
		// string UnityEngine._AndroidJNIHelper.GetSignature<object>(object[])
	}
}