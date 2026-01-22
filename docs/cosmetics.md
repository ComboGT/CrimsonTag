# How Cosmetics Work in GT Crimson Tag

Hey! This guide explains how the cosmetics system (hats, badges, holdables, etc.) works in the game. Think of it like a guide to how your closet works!

## The Big Picture

Imagine the game has two parts working together:

1. **PlayFab** - This is like a store's computer system in the cloud. It keeps track of what items exist and which players own what items.

2. **Unity (the game)** - This is what runs on your computer. It shows the 3D models and lets you wear your items.

When you play the game, it asks PlayFab: "Hey, what items does this player own?" Then it lets you wear only those items.

---

## What is a Cosmetic Item?

Every cosmetic item (like a hat or badge) has these properties:

| Property | What it means |
|----------|---------------|
| `itemName` | The item's ID (like "finger painter") - this matches what PlayFab calls it |
| `itemCategory` | What type of item it is (Hat, Badge, Face, Holdable, Gloves) |
| `itemPicture` | The little picture you see in the menu |
| `displayName` | The name players see (like "Finger Painter Badge") |
| `cost` | How many Shiny Rocks it costs |
| `canTryOn` | Can you try it on before buying? |

---

## Item Categories (Types of Cosmetics)

The game has these categories:

| Number | Category | Where it goes |
|--------|----------|---------------|
| 1 | Hat | On your head |
| 2 | Badge | On your chest |
| 3 | Face | On your face |
| 4 | Holdable | In your hands or on your back |
| 5 | Gloves | On your hands |
| 6 | Slingshot | Special slingshot skins |
| 8 | Set | A bundle of multiple items |

---

## How Does the Game Know What You Own?

Here's what happens step by step:

### Step 1: Ask PlayFab
When you start the game, it calls PlayFab and says "Give me this player's inventory!"

The code that does this is in `CosmeticsController.cs`:
```csharp
PlayFabClientAPI.GetUserInventory(...)
```

### Step 2: Make a List
PlayFab sends back a list of items you own. The game puts all these item names together into one big string (like a long sentence).

For example, if you own a "finger painter" badge and a "top hat", your string might look like:
```
"finger painter, top hat"
```

This string is stored in `VRRig.concatStringOfCosmeticsAllowed`.

### Step 3: Check When Equipping
When you try to wear an item, the game checks: "Is this item name in your allowed string?"

```csharp
if (concatStringOfCosmeticsAllowed.Contains(itemName))
    return true;  // Yes, you can wear it!
```

It's like checking if your name is on a guest list!

---

## How Equipping Works

The game has 10 "slots" where you can wear things:

| Slot | What goes there |
|------|-----------------|
| 0 | Hat |
| 1 | Badge |
| 2 | Face |
| 3 | Left arm holdable |
| 4 | Right arm holdable |
| 5 | Back left holdable |
| 6 | Back right holdable |
| 7 | Left hand glove |
| 8 | Right hand glove |
| 9 | Chest holdable |

When you equip something, the game:
1. Figures out which slot it goes in (based on category)
2. Turns OFF the old item's 3D model
3. Turns ON the new item's 3D model
4. Saves your choice so it remembers next time

---

## Adding a New Cosmetic Item (Like a Badge)

Here's the cool part - you don't need to write special code for each new item! The system is "data-driven" which means you just need to add data, not code.

### What You Need to Add a New Badge:

#### 1. Create the 3D Model
Make a prefab (3D model file) and put it in:
```
Assets/creatorbadges/tier1/YourBadgeName.prefab
```

The name of the file matters! The game uses it to find the right model.

#### 2. Add it to PlayFab
In PlayFab (the online dashboard), add a new item to the "DLC" catalog with:
- An `ItemId` (like "cool badge")
- A price in Shiny Rocks ("SR")

#### 3. Add to the allcosmetics String
Open `GorillaGameManager.cs` and find the `allcosmetics` string (around line 37). Add your badge name:

```csharp
// Before:
public static string allcosmetics = "... 1 finger painter, 1 LHABV., ..."

// After (added "cool badge"):
public static string allcosmetics = "... 1 finger painter, 1 cool badge, 1 LHABV., ..."
```

#### 4. Add to Unity Inspector
Open the CosmeticsController in Unity and add your item to the `allCosmetics` list with:
- `itemName`: "cool badge" (must match PlayFab!)
- `itemCategory`: Badge (which is 2)
- `itemPicture`: Your sprite image
- `displayName`: "Cool Badge"
- `cost`: 0 (PlayFab will update this)

That's it! No special code needed!

---

## Important Files to Know

| File | What it does |
|------|--------------|
| `CosmeticsController.cs` | The main boss - handles everything about cosmetics |
| `VRRig.cs` | Controls what your gorilla looks like and wears |
| `GorillaGameManager.cs` | Has the list of all cosmetic names |
| `CosmeticItemRegistry.cs` | Connects item names to their 3D models |

All these files are in:
```
Assets/Scripts/Assembly-CSharp/
```

---

## Quick Reference: The Flow

```
┌─────────────────┐
│  Game Starts    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Ask PlayFab:    │
│ "What do I own?"│
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Build ownership │
│ string list     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Player picks    │
│ item to equip   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Check: Is item  │
│ in my list?     │
└────────┬────────┘
         │
    ┌────┴────┐
    │         │
   YES        NO
    │         │
    ▼         ▼
┌───────┐  ┌───────┐
│ Equip │  │ Sorry!│
│ item  │  │ Can't │
└───────┘  └───────┘
```

---

## Glossary (Big Words Explained)

- **PlayFab**: A cloud service that stores player data online
- **Prefab**: A ready-made 3D object in Unity
- **Inventory**: The list of items a player owns
- **Catalog**: The list of ALL items that exist in the game
- **Data-driven**: A system where you add data (not code) to make changes
- **Serialized**: Saved in a way Unity can read and edit in the Inspector

---

## Tips for Working with Cosmetics

1. **Item names must match everywhere** - The name in PlayFab, the code, and the prefab must all match exactly!

2. **Test in Try-On mode** - Items with `canTryOn = true` can be tested without owning them (in certain areas)

3. **Check the Console** - If an item doesn't show up, Unity's console might tell you why

4. **Prefab naming matters** - Use `LEFT.itemname` and `RIGHT.itemname` for items that go on both sides

---

Good luck and have fun making cool cosmetics!
