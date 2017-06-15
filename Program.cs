using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack
{    
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
    //Card class - Suit, Face, Value
    public class Card
    {
        public Suit Suit { get; set; }
        public Face Face { get; set; }
        public int Value { get; set; }
    }
    //Deck class - Build, Shuffle
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
                    //make ace 11
                    if (j == 0)
                        cards[cards.Count - 1].Value = 11;
                    else if (j <= 8)
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
    //Hand Class - Add
    public class Hand
    {
        public List<Card> newHand { get; set; }
        public Hand()
        {
            this.newHand = new List<Card>();
        }
        public void Add(Card card)
        {
            newHand.Add(card);
        }
    }
    //Player Class - Hit, Stay, AceValue?
    //Player class bug - hits over 21 are subtracted?
    public class Player
    {
        public bool CheckInput(string input, Deck deck, Hand playerHand)
        {
            if (input.ToUpper() == "S")
                return true;

            if (input.ToUpper() == "H")
            {
                Hit(deck, playerHand);
                if (ScoreCheck(playerHand) > 21)
                {
                    for (int i = 0; i < playerHand.newHand.Count(); i++)
                    {
                        if (playerHand.newHand[i].Value == 11)
                            playerHand.newHand[i].Value = 1;
                    }
                    if (ScoreCheck(playerHand) > 21)
                    {
                        return false;
                    }
                    return false;
                }
                else
                {
                    Console.Write("Player hand: ");
                    for (int i = 0; i < playerHand.newHand.Count(); i++)
                    {
                        Console.Write($"{playerHand.newHand[i].Suit} {playerHand.newHand[i].Face} ");
                    }
                    
                    int playerScore = 0;
                    for (int i = 0; i < playerHand.newHand.Count(); i++)
                    {
                        playerScore = playerScore + playerHand.newHand[i].Value;
                    }
                    Console.WriteLine($"Player score: {playerScore}");
                    Console.Write("(H)it or (S)tay: ");
                    string anotherUserAction = Console.ReadLine();
                    return this.CheckInput(anotherUserAction, deck, playerHand);
                }
            }


            return false;     

        }
        public Hand Hit(Deck deck, Hand hand)
        {
            hand.Add(deck.cards[0]);
            deck.cards.Remove(deck.cards[0]);

            return hand;
        }
        public int ScoreCheck(Hand playerHand)
        {
            int total = 0;
            for (int i = 0; i < playerHand.newHand.Count(); i++)
            {
                total = total + playerHand.newHand[i].Value;
            }
            return total;
        }
    }
    //Dealer class - DealCards
    public class Dealer : Player
    {
        public Hand Deal(Deck deck, Hand hand)
        {
            //deck.cards.Take(2);
            for (int i = 0; i < 2; i++)
            {
                hand.Add(deck.cards[0]);
                deck.cards.Remove(deck.cards[0]);
           
            }

            return hand;
        }
        public bool Play(Deck deck, Hand hand)
        {
            int score = 0;
            Console.Write($"Dealer hand: ");
            for (int i = 0; i < hand.newHand.Count(); i++)
            {
                score = score + hand.newHand[i].Value;
                Console.Write($"{hand.newHand[i].Suit} {hand.newHand[i].Face}");
            }
            Console.WriteLine($"Dealer score: {score} ");
            if (score > 21)
            {
                Console.WriteLine("Dealer busted.");
                return false;
            } else if (score > 17)
            {
                Console.WriteLine("Dealer stays.");
                return true;
            } else
            {
                Hit(deck, hand);
                return Play(deck, hand);
            }

        }
    }
    class Program
    {
        //static List<Card> userHand = new List<Card>();
        //static List<Card> dealerHand = new List<Card>();

        static char restart;
        static void Main(string[] args)
        {
            do
            {
                // Start game
                Console.WriteLine("Welcome to Blackjack!");
                //shuffle deck
                var deck = new Deck();
                deck.Shuffle();
                //deal cards
                var dealer = new Dealer();
                Hand playerHand = new Hand();
                Hand compHand = new Hand();
                //player gets cards
                dealer.Deal(deck, playerHand);
                int playerScore = playerHand.newHand[0].Value + playerHand.newHand[1].Value;
                Console.WriteLine($"Player hand: {playerHand.newHand[0].Suit} {playerHand.newHand[0].Face} {playerHand.newHand[1].Suit} {playerHand.newHand[1].Face}");
                Console.WriteLine($"Player score: {playerScore}");
                //dealer gets cards
                dealer.Deal(deck, compHand);
                int compScore = compHand.newHand[0].Value + compHand.newHand[1].Value;
                Console.WriteLine($"Dealer hand: {compHand.newHand[0].Suit} {compHand.newHand[0].Face} {compHand.newHand[1].Suit} {compHand.newHand[1].Face}");
                Console.WriteLine($"Dealer score: {compScore}");
                Console.WriteLine($"Number of cards left: {deck.cards.Count()}");
                //does everyone have 21? tie game
                if (compScore == 21 && playerScore == 21)
                {
                    Console.WriteLine("Both players have 21! Tie!");
                }
                //does dealer have 21? dealer win
                else if (compScore == 21)
                {
                    Console.WriteLine("Blackjack for the Dealer! 21!");
                }
                //does player have 21? player win
                else if (playerScore == 21)
                {
                    Console.WriteLine("Blackjack for you! 21! You win!");
                }
                else
                {
                    //class Player.Hit(), Stay() if 21 autostay, if score > 21 switch Ace to 1
                    var user = new Player();
                    Console.Write("(H)it or (S)tay: ");
                    string input = Console.ReadLine();
                    var userPlay = user.CheckInput(input, deck, playerHand);
                    if(userPlay == true)
                    {
                        Console.WriteLine("Computer turn!");
                        var dealerPlay = dealer.Play(deck, compHand);
                        if(dealerPlay == true)
                        {
                            //resolve scores
                            Console.WriteLine("scores are vs");
                        }
                        else
                        {
                            Console.WriteLine("You win! Dealer busted.");
                        }
                    } else
                    {
                        Console.WriteLine("You busted! Over 21");
                    }

                    Console.Write("Player hand: ");
                    for (int i = 0; i < playerHand.newHand.Count(); i++)
                    {
                        Console.Write($"{playerHand.newHand[i].Suit} {playerHand.newHand[i].Face} ");
                    }
                    Console.WriteLine($"Player score: {playerScore}");

                }

                Console.WriteLine("Play again? y/n");
                restart = Convert.ToChar(Console.ReadLine());
            } while (restart == 'y');

        }
    }
}
