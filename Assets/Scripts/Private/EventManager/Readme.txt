In KickSomeBass, the fight phase strictly follows the Event-driven architecture.
All the fight modules(such as Ability System, UI System, Audio System, etc) are fully decoupled and only communicates through the Event System.

Event System uses the hybrid of pub/sub and observer design pattern.