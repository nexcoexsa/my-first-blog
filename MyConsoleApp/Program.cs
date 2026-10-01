using System;
using System.Collections.Generic;
using System.Linq; // ← LINQを使うための必須の命令です！

Console.WriteLine("=====================================");
Console.WriteLine("⚔️ LINQ応用：パーティーのステータス管理と救出");
Console.WriteLine("=====================================\n");

// 1. キャラクターのリスト（パーティー）を作成
List<Character> party = new List<Character>
{
    new Character("勇者ロト", 120, "戦士職"),
    new Character("魔法使いウィズ", 35, "魔法職"), // ピンチ！
    new Character("僧侶セシリア", 48, "回復職"),   // ピンチ！
    new Character("武闘家ハッサン", 180, "戦士職"),
    new Character("遊び人ゴエモン", 0, "その他")   // 死亡（HP 0）
};

Console.WriteLine("--- 📋 現在のパーティーの全ステータス ---");
foreach (var member in party)
{
    Console.WriteLine($"・[{member.Job}] {member.Name} (HP: {member.Hp})");
}


// --------------------------------------------------
// 応用1. Where を使って「HPが50以下の生きているピンチな仲間」を抽出
// --------------------------------------------------
// 条件：HPが0より大きい（生存） かつ HPが50以下
var pinchedMembers = party.Where(c => c.Hp > 0 && c.Hp <= 50);

Console.WriteLine("\n🚨 🚨 【LINQ警告】 HPが50以下のピンチな仲間 🚨 🚨");
foreach (var member in pinchedMembers)
{
    Console.WriteLine($"👉 {member.Name} の体力が残りわずかです！ (HP: {member.Hp})");
}


// --------------------------------------------------
// 応用2. Count を使って「生存している人数」を数える
// --------------------------------------------------
int aliveCount = party.Count(c => c.Hp > 0);
Console.WriteLine($"\n👥 生存している戦闘可能メンバー: {aliveCount} / {party.Count} 人");


// --------------------------------------------------
// 応用3. Average を使って「生存者の平均HP」を計算
// --------------------------------------------------
// 生存しているメンバーだけに絞り込んでから、その人たちのHpプロパティの平均を出します
double averageHp = party.Where(c => c.Hp > 0).Average(c => c.Hp);
Console.WriteLine($"💚 生存メンバーの平均HP: {averageHp:F1}");


// --------------------------------------------------
// 応用4. Any を使って「全滅（全員HP 0）しているか」を一発判定
// --------------------------------------------------
// Anyは「条件に合うデータが1つでもあるか？」を true/false で返すLINQです
// 「生存している人が1人もいない ＝ 全滅」というロジックです
bool isWipedOut = !party.Any(c => c.Hp > 0);
if (isWipedOut)
{
    Console.WriteLine("\n💀 全滅しました...");
}
else
{
    Console.WriteLine("\n🛡️ まだ全滅していません！戦いを続けられます。");
}

Console.WriteLine("\n=====================================");


// --------------------------------------------------
// キャラクターを表す設計図（クラス）
// --------------------------------------------------
class Character
{
    public string Name { get; set; }
    public int Hp { get; set; }
    public string Job { get; set; }

    public Character(string name, int hp, string job)
    {
        Name = name;
        Hp = hp;
        Job = job;
    }
}
