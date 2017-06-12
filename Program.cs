using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack
{

    // suit enum
    // card number enums
    // loop over and assign values
    //randomize card deck

    public enum Suit
    {
        Heart,
        Diamond,
        Spade,
        Club
    }
    public enum Face
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
    }

    public class Card
    {
        public Suit Suit { get; set; }
        public Face Face { get; set; }
        public int Value { get; set; }
    }

    public class Deck
    {
        public List<Card> cards;

        public Deck()
        {
            this.BuildDeck();
        }

        public void BuildDeck()
        {
            cards = new List<Card>();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    cards.Add(new Card() { Suit = (Suit)i, Face = (Face)j });

                    if (j <= 8)
                        cards[cards.Count - 1].Value = j + 1;
                    else
                        cards[cards.Count - 1].Value = 10;
                }
            }
        }

        public void Shuffle()
        {
            Random rng = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card card = cards[k];
                cards[k] = cards[n];
                cards[n] = card;
            }
        }



    

        public void PrintDeck()
        {
            int i = 1;
            foreach (Card card in cards)
            {
                Console.WriteLine("Card {0}: {1} of {2}. Value: {3}", i, card.Face, card.Suit, card.Value);
                i++;
            }
        }



    }

    class Program
    {
        static void Main(string[] args)
        {
            // Start game, deal

            //player hit or stay

            //hit? bust? lose

            //computer plays if player is <= 21

            //declare winner

            //deal next hand

            //Console.Write("Enter number: ");
            //var input = Console.ReadLine();
            //var num = Convert.ToInt32(input);
            //Console.WriteLine($"echo: {num}");

            var deck = new Deck();

            deck.Shuffle();

            deck.PrintDeck();

            Console.ReadLine();

        }
    }
}
