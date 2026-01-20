using System;

const int STANDARD_DECK_SIZE = 52;
const int DECKS = 8;
const int CUTOFF = 14;
const int DEFAULT_HAND_SIZE = 2;
const int BET_MIN = 100;
const int BET_MAX = 15000;

int shoeSize = STANDARD_DECK_SIZE * DECKS;
List<Card> shoe = new List<Card>();

Random random = new Random();

List<Card> playerHand = new List<Card>();
List<Card> bankerHand = new List<Card>();

int playerTotal = 0;
int bankerTotal = 0;

int handsPlayed = 0;
bool isLastHand = false;

// Set up the shoe
for (int i = 0; i < shoeSize; i += STANDARD_DECK_SIZE)
    for (int j = 0; j < Enum.GetNames(typeof(Rank)).Length; j++)
    {
        shoe.Add(new Card((Rank)j, Suit.Hearts));
        shoe.Add(new Card((Rank)j, Suit.Diamonds));
        shoe.Add(new Card((Rank)j, Suit.Clubs));
        shoe.Add(new Card((Rank)j, Suit.Spades));
    }

// Gameplay!
// TODO: Figure out side bets (player/banker pair, tie)
while (!isLastHand)
{
    PlaceBet();

    DrawPlayerAndBankerCards();

    DetermineDrawMechanics();

    ShowResults();
}

Console.WriteLine("Shoe has ended. Thank you for playing!");

void PlaceBet()
{
    bool betPlaced = false;

    while (!betPlaced)
    {
        Console.WriteLine("Place your bet: ");

        if (!double.TryParse(Console.ReadLine(), out double betSize))
        {
            Console.WriteLine("Bets must be a number.");
            continue;
        }
        else if (betSize > BET_MAX && betSize < BET_MIN)
        {
            Console.WriteLine($"Bet value must be between ${BET_MIN} and ${BET_MAX}.");
            continue;
        }
        else
        {
            BetOnSides();

            Console.WriteLine("No more bets. Good luck!");
            betPlaced = true;
        }
    }    
}

void BetOnSides()
{
    Console.Clear();

    Console.WriteLine()
    Console.WriteLine("Would you like to make a side bet?");
    
    return;
}

void DrawPlayerAndBankerCards()
{
    // First draw goes to player, then each draw alternates
    for (int i = 0; i < DEFAULT_HAND_SIZE; i++)
    {
        CheckForCutoffCard();

        int index = random.Next(shoe.Count);
        playerHand.Add(shoe[index]);

        playerTotal += shoe[index].Rank;
        if (playerTotal >= 10)
            playerTotal -= 10;

        shoe.RemoveAt(index);

        CheckForCutoffCard();

        index = random.Next(shoe.Count);
        bankerHand.Add(shoe[index]);

        bankerTotal += shoe[index].Rank;
        if (bankerTotal >= 10)
            bankerTotal -= 10;

        shoe.RemoveAt(index);
    }
}

void DetermineDrawMechanics()
{
    // No more cards
    if (playerTotal >= 8 || bankerTotal >= 8 || (playerTotal == 6 && bankerTotal == 7) || (playerTotal == 7 && bankerTotal == 6))
        return;
    if (playerTotal < 6) // Player draws
    {
        DrawThirdPlayerCard();
    }
    else // Banker draws
    {
        DrawThirdBankerCard();
    }
}

void DrawThirdPlayerCard()
{
    CheckForCutoffCard();

    int index = random.Next(shoe.Count);
    playerHand.Add(shoe[index]);
    
    playerTotal += shoe[index].Rank;
    if (playerTotal >= 10)
        playerTotal -= 10;

    shoe.RemoveAt(index);

    // All cases where banker draws one more card
    switch (bankerTotal)
    {
        default: // Totals 0-2
            DrawThirdBankerCard();
            break;
        case 3:
            if (playerHand[2].Rank != 8)
                DrawThirdBankerCard();
            break;
        case 4:
            if (playerHand[2].Rank >= 2 && playerHand[2].Rank <= 7)
                DrawThirdBankerCard();
            break;
        case 5:
            if (playerHand[2].Rank >= 4 && playerHand[2].Rank <= 7)
                DrawThirdBankerCard();
            break;
        case 6:
            if (playerHand[2].Rank == 6 || playerHand[2].Rank == 7)
                DrawThirdBankerCard();
            break;
    }
}

void DrawThirdBankerCard()
{
    // Second draw goes to banker
    CheckForCutoffCard();

    int index = random.Next(shoe.Count);
    bankerHand.Add(shoe[index]);

    bankerTotal += shoe[index].Rank;
    if (bankerTotal >= 10)
        bankerTotal -= 10;

    shoe.RemoveAt(index);
}

void CheckForCutoffCard()
{
    if (shoe.Count == CUTOFF)
    {
        // If player hasn't gotten any cards yet, it's the last hand; otherwise, second-last
        if (playerHand.Count == 0)
        {
            Console.WriteLine("Last hand of the shoe!");
            isLastHand = true;
        }
        else
            Console.WriteLine("Second-last hand of the shoe!");
    }
}

void ShowResults()
{
    Console.Clear();

    Console.WriteLine("============================");
    Console.WriteLine("         Cards Out!");
    Console.WriteLine("============================\n\n");

    Console.WriteLine("    Player             Banker");
    Console.WriteLine("    {0} {1}              {2} {3}", playerHand[0].Rank.ToString() + playerHand[0].Suit, playerHand[1].Rank.ToString() + playerHand[1].Suit, bankerHand[0].Rank.ToString() + bankerHand[0].Suit, bankerHand[1].Rank.ToString() + bankerHand[1].Suit);

    // Extra cards (if needed)
    if (playerHand.Count == 3 && bankerHand.Count == 2)
        Console.WriteLine("      {0}", playerHand[2].Rank.ToString() + playerHand[2].Suit);
    else if (playerHand.Count == 2 && bankerHand.Count == 3)
        Console.WriteLine("                         {0}", bankerHand[2].Rank.ToString() + bankerHand[2].Suit);
    else if (playerHand.Count == 3 && bankerHand.Count == 3)
        Console.WriteLine("      {0}                {1}", playerHand[2].Rank.ToString() + playerHand[2].Suit, bankerHand[2].Rank.ToString() + bankerHand[2].Suit);

    if (playerTotal > bankerTotal)
        Console.WriteLine($"Player wins, {playerTotal} over {bankerTotal}.");
    else if (bankerTotal > playerTotal)
        Console.WriteLine($"Banker wins, {bankerTotal} over {playerTotal}.");
    else
        Console.WriteLine($"Player and banker tie with a value of {playerTotal}.");

    // Reset totals and hands
    playerTotal = 0;
    bankerTotal = 0;

    playerHand = new List<Card>();
    bankerHand = new List<Card>();

    handsPlayed++;
}