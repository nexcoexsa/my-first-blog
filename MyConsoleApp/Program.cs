using System;
using System.Collections.Generic;
using System.Linq;

Console.WriteLine("==================================================");
Console.WriteLine("⚔️  C#総決算：本格ターン制RPGバトルゲーム ⚔️");
Console.WriteLine("==================================================\n");

// 1. 味方パーティーの作成（リスト管理）
List<Battler> allies = new List<Battler>
{
    new Warrior("勇者ロト", 180, 30, 12),     // HP, 攻撃力, 素早さ
    new Wizard("魔法使いウィズ", 100, 15, 10, 50) // HP, 攻撃力, 素早さ, MP
};

// 2. 敵グループの作成（リスト管理）
List<Battler> enemies = new List<Battler>
{
    new Battler("スライムA", 40, 10, 8),
    new Battler("ドラゴン", 250, 35, 11),
    new Battler("スライムB", 40, 10, 5)
};

int turn = 1;

// 3. メインゲームループ（どちらかのチームが全滅するまで繰り返す）
while (allies.Any(a => a.IsAlive) && enemies.Any(e => e.IsAlive))
{
    Console.WriteLine($"\n================== ターン {turn} ==================");
    
    // LINQを使って、生存している全員を「素早さ（Agility）が高い順」に並び替えてタイムラインを作る！
    var timeline = allies.Concat(enemies)
                         .Where(b => b.IsAlive)
                         .OrderByDescending(b => b.Agility)
                         .ToList();

    foreach (var activeUnit in timeline)
    {
        // 自分の手番が回ってきたときに、すでに倒れていたらスキップ
        if (!activeUnit.IsAlive) continue;

        // 敵の全滅、または味方の全滅を一発チェック
        if (!allies.Any(a => a.IsAlive) || !enemies.Any(e => e.IsAlive)) break;

        Console.WriteLine($"\n[ {activeUnit.Name} の行動ターン ] (素早さ:{activeUnit.Agility})");

        // 味方キャラクターの場合（コマンド入力）
        if (allies.Contains(activeUnit))
        {
            // LINQで生存している敵だけをターゲット候補にする
            var aliveEnemies = enemies.Where(e => e.IsAlive).ToList();
            
            // 行動選択ループ（例外・入力エラーを try-catch 形式で安全にガード）
            bool validAction = false;
            while (!validAction)
            {
                try
                {
                    Console.WriteLine("▼ 行動を選んでください:");
                    if (activeUnit is Wizard)
                        Console.WriteLine("1: 通常攻撃 / 2: 魔法攻撃(メラゾーマ・MP15消費)");
                    else
                        Console.WriteLine("1: 通常攻撃 / 2: 全力斬り(反動で自分も5ダメージ)");

                    Console.Write("選択（1 または 2）: ");
                    string? cmd = Console.ReadLine();

                    Console.WriteLine("\n▼ 攻撃する対象を選んでください:");
                    for (int i = 0; i < aliveEnemies.Count; i++)
                    {
                        Console.WriteLine($"{i}: {aliveEnemies[i].Name} (HP:{aliveEnemies[i].Hp})");
                    }
                    Console.Write("対象の番号: ");
                    int targetIdx = int.Parse(Console.ReadLine() ?? "0");

                    // ターゲットを決定
                    Battler targetIndex = aliveEnemies[targetIdx];

                    // スキル分岐と実行
                    if (cmd == "2")
                    {
                        if (activeUnit is Wizard w) w.CastSpell(targetIndex);
                        else if (activeUnit is Warrior wa) wa.MightyStrike(targetIndex);
                    }
                    else
                    {
                        activeUnit.Attack(targetIndex);
                    }
                    validAction = true; // 正常に終了したらループを抜ける
                }
                catch (Exception)
                {
                    Console.WriteLine("⚠️ 入力が正しくありません。もう一度やり直してください。\n");
                }
            }
        }
        // 敵キャラクターの場合（AIの自動ランダム攻撃）
        else
        {
            var aliveAllies = allies.Where(a => a.IsAlive).ToList();
            Random rand = new Random();
            Battler randomTarget = aliveAllies[rand.Next(aliveAllies.Count)];
            activeUnit.Attack(randomTarget);
        }
    }
    turn++;
    Console.WriteLine("\n--------------------------------------------------");
}

// 4. ゲームの決着（勝敗判定）
Console.WriteLine("\n==================================================");
if (allies.Any(a => a.IsAlive))
{
    Console.WriteLine("🎉 🎉 【VICTORY】 魔王軍に勝利した！平和が訪れた！ 🎉 🎉");
}
else
{
    Console.WriteLine("💀 💀 【GAME OVER】 パーティーは全滅してしまった... 💀 💀");
}
Console.WriteLine("==================================================");


// ==================================================
// 🧱 登場人物・設計図（クラス群）
// ==================================================

// 1. 基底親クラス（すべてのバトラーの基本）
class Battler
{
    public string Name { get; set; }
    public int Agility { get; set; } // 素早さ（ターンの順番に影響）
    public int AttackPower { get; set; }

    private int _hp;
    public int Hp
    {
        get => _hp;
        set => _hp = Math.Max(0, value); // カプセル化によるHPの下限ガード（0以下にならない）
    }

    public bool IsAlive => Hp > 0; // 生存フラグプロパティ

    public Battler(string name, int hp, int attackPower, int agility)
    {
        Name = name;
        Hp = hp;
        AttackPower = attackPower;
        Agility = agility;
    }

    public virtual void Attack(Battler target)
    {
        Console.WriteLine($"⚔️ {Name} の攻撃！ {target.Name} に {AttackPower} のダメージ！");
        target.Hp -= AttackPower;
    }
}

// 2. 子クラス：戦士（Warrior）
class Warrior : Battler
{
    public Warrior(string name, int hp, int attackPower, int agility) : base(name, hp, attackPower, agility) { }

    // 戦士独自の必殺技
    public void MightyStrike(Battler target)
    {
        int damage = AttackPower + 20;
        Console.WriteLine($"🪓 {Name} の豪快な全力斬り！ {target.Name} に {damage} の痛烈なダメージ！");
        target.Hp -= damage;
        Console.WriteLine($"   [反動] {Name} も反動で 5 のダメージを受けた！");
        this.Hp -= 5;
    }
}

// 3. 子クラス：魔法使い（Wizard）
class Wizard : Battler
{
    public int Mp { get; set; }

    public Wizard(string name, int hp, int attackPower, int agility, int mp) : base(name, hp, attackPower, agility)
    {
        Mp = mp;
    }

    // 魔法使い専用の「ステータス画面上書き」
    public override void Attack(Battler target)
    {
        Console.WriteLine($"🪄 {Name} は杖でポカポカ叩いた！ {target.Name} に {AttackPower} のダメージ！");
        target.Hp -= AttackPower;
    }

    // 魔法使い独自の呪文
    public void CastSpell(Battler target)
    {
        if (Mp >= 15)
        {
            Mp -= 15;
            int damage = AttackPower * 3;
            Console.WriteLine($"🔮 {Name} は大魔法【メラゾーマ】を放った！（MP-15）");
            Console.WriteLine($"   {target.Name} に {damage} の超絶炎ダメージ！ (残りMP: {Mp})");
            target.Hp -= damage;
        }
        else
        {
            Console.WriteLine($"💬 {Name} は呪文を唱えようとしたが、MPが足りない！通常攻撃に切り替えます。");
            Attack(target);
        }
    }
}
