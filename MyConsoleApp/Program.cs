using System;
using System.Collections.Generic;

Console.WriteLine("=====================================");
Console.WriteLine("🌟 オーバーライド(override)と集大成");
Console.WriteLine("=====================================\n");

// 1. リストを使って、異なる職業のキャラクターをまとめて管理
List<Character> party = new List<Character>();
party.Add(new Character("戦士ガッツ", 200, 30));
party.Add(new Wizard("魔法使いウィズ", 100, 15, 50));

// 2. まとめてステータスを表示
// 魔法使いは自動的に「上書きされた新しいShowStatus」が動きます！
foreach (Character member in party)
{
    member.ShowStatus();
}

Console.WriteLine("\n=====================================");


// --------------------------------------------------
// 親クラス（Character）
// --------------------------------------------------
class Character
{
    public string Name { get; set; }
    public int Hp { get; set; }
    public int AttackPower { get; set; }

    public Character(string name, int hp, int attackPower)
    {
        Name = name;
        Hp = hp;
        AttackPower = attackPower;
    }

    // ⭐ ポイント1: 子クラスで上書きを許可するメソッドには「virtual（バーチャル）」をつけます
    public virtual void ShowStatus()
    {
        Console.WriteLine($"[戦士職] {Name} / HP: {Hp} / 攻撃力: {AttackPower}");
    }
}

// --------------------------------------------------
// 子クラス（Wizard）
// --------------------------------------------------
class Wizard : Character
{
    public int Mp { get; set; }

    public Wizard(string name, int hp, int attackPower, int mp) : base(name, hp, attackPower)
    {
        Mp = mp;
    }

    // ⭐ ポイント2: 親のメソッドを書き換えるときは「override（オーバーライド）」をつけます
    public override void ShowStatus()
    {
        Console.WriteLine($"[魔法職] {Name} / HP: {Hp} / MP: {Mp} / 攻撃力: {AttackPower}");
    }
}
