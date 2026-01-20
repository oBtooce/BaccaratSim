using System;

public class Card
{
	public Card(Rank rank, Suit suit)
	{
        switch ((int)rank)
        {
            case 10:
            case 11:
            case 12:
            case 13:
                Rank = 0;
                break;
            default:
                Rank = (int)rank;
                break;
        }
		switch ((int)suit)
        {
            case 0:
                Suit = "♥";
                break;
            case 1:
                Suit = "♦";
                break;
            case 2:
                Suit = "♣";
                break;
            case 3:
                Suit = "♠";
                break;
        }
	}

	public int Rank {  get; set; }

	public string Suit { get; set; }
}

public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

public enum Rank
{
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13
}
