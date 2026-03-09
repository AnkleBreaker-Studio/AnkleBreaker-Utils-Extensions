# AnkleBreaker Studio - Utils Extensions

[![Sponsor](https://img.shields.io/badge/Sponsor-AnkleBreaker%20Studio-red?logo=github)](https://github.com/sponsors/AnkleBreaker-Studio)
[![Asset Store](https://img.shields.io/badge/Asset%20Store-AnkleBreaker%20Studio-blue)](https://assetstore.unity.com/publishers/101837)

Extension methods for built-in Unity and C# types, helpers, and utility structs.

## Installation

Add via Unity Package Manager using the Git URL:

```
https://github.com/AnkleBreaker-Studio/AnkleBreaker-Utils-Extensions.git
```

## Contents

### Extension Methods — Built-in Types

AnimationCurve, Collider, Color, Color32, ContentSizeFitter, DateTime, Debug, Dictionary, Enumerable, EventSystem, Float, GameObject, Int, LayerMask, List, Object, Quaternion, Queue, RectTransform, SkinnedMeshRenderer, String, Texture2D, Transform, UInt

### Extension Methods — AB Types

PositionRotation extensions (transform-equivalent operations without a Transform component)

### Helpers

- `FlagsHelper` — generic bitwise flag operations (IsSet, Set, Unset, Toggle, etc.)

### Random

- `AB_Random` — extended random utilities (Range with exclusion, weighted random, shuffling)

### Type Definitions

- `PositionRotation` — serializable struct for position + rotation pairs

## Requirements

- Unity 2022.3 LTS or later

## License

See [LICENSE.md](LICENSE.md)