Hello! Here's a horizontal infinite scroll rect using UGUI's scroll rect. 

Cards put into the Deck Class is any order are pulled into the InfiniteScroll Class where they are sorted based on their public values in the following order: (Hearts, Diamonds, Clubs then Spades) and value (with ace being the highest, and 2 being the least)

To create this effect. Each card is instantiated three times, there's an original, and two more on the left and right to create an infinite-like effect.

Around 4 cards are visible at one time, and the rest are masked on the outer edges.
