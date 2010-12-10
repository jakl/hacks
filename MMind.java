import java.util.Random;
import java.util.Scanner;
import java.util.regex.Pattern;

public class MMind {

	public static void main(String[] args) {

		boolean badLetter, badNumber, badSeed;
		String sNumber, sSeed;
		char letter;
		Scanner in = new Scanner(System.in);

		do {
			System.out.print("Enter max letter, number of letters and seed: ");

			//letter
			Pattern pLetter = Pattern
			 .compile("[A-F]", Pattern.CASE_INSENSITIVE);
			letter = in.hasNext(pLetter) ? in.next().charAt(0) : 0;// Sentinel 0
			letter = Character.toUpperCase(letter);
			badLetter = letter == 0;
			if (badLetter)
				in.next();// throw away bad input on the input line
			
			//number
			sNumber = in.hasNext("[1-9]|10") ? in.next() : null;
			badNumber = sNumber == null;
			if (badNumber)
				in.next();
			
			//seed
			sSeed = in.hasNext("\\d+") ? in.next() : null;
			badSeed = sSeed == null;
			if (badSeed)
				in.next();

			//error messages
			if (badLetter && !badNumber && !badSeed)
				System.out.println("Max letter must be between A and F");
			else if (badNumber && !badLetter && !badSeed)
				System.out
						.println("Number of letters must be between 1 and 10");
			else if (badNumber || badLetter || badSeed)
				System.out.println("Bad format for one or more values");
		} while (badLetter || badNumber || badSeed);
		in.nextLine();// gobble left over trailing characters or new lines

		Random rnd = new Random(Long.parseLong(sSeed));
		game(letter, Integer.parseInt(sNumber), rnd, in);
	}

	static void game(char letter, int number, Random rnd, Scanner in) {
		int iGuesses = 0;
		int iGame = 0;
		boolean anothergame;

		do {// each game
			System.out.println("\nStarting game...");
			iGame++;
			
			int iLetter = Character.toUpperCase(letter) - 'A' + 1;
			String answer = "";
			for (int i = 0; i < number; i++)
				answer += (char) (Math.abs(rnd.nextInt(iLetter)) + 'A');
			System.out.println("Pattern is: " + answer);

			int iGuess = 0;
			boolean tryagain = false;
			boolean anotherguess = true;

			do {// each guess
				if (tryagain)
					System.out.print("Try again: ");
				else
					System.out.print("Enter guess  " + (++iGuess) + ": ");
				tryagain = false;

				// grab guess while deleting space characters
				String guess = "";
				for (char c : in.nextLine().toCharArray())
					if (c != ' ')
						guess += Character.toUpperCase(c);

				// validate guess
				if (guess.length() != answer.length()
						|| !guess.matches("[A-" + letter + "]+")) {
					System.out.println("Pattern must have exactly "
					 + answer.length()
					 + " characters, all between A and " + letter);
					tryagain = true;
					continue;
				}
				System.out.println(getExactInexact(answer, guess));
				anotherguess = !guess.equalsIgnoreCase(answer);
			} while (anotherguess);
			iGuesses += iGuess;

			System.out.printf(
			 "Pattern found in %d attempts.  Current average:  %5.3f",
			 iGuess, (float) iGuesses / (float) iGame);

			System.out.print("\n\nAnother game [Y/N]? ");
			anothergame = in.nextLine().equalsIgnoreCase("y");
		} while (anothergame);
	}

	static String getExactInexact(String answer, String guess) {

		boolean[] usedanswer = new boolean[answer.length()];
		int exact = 0;

		// exact
		for (int i = 0; i < answer.length(); i++) {
			usedanswer[i] = answer.charAt(i) == guess.charAt(i);
			if (usedanswer[i])
				exact++;
		}

		boolean[] usedguess = usedanswer.clone();
		int inexact = 0;

		// inexact
		for (int i = 0; i < answer.length(); i++) {
			if (usedanswer[i])
				continue;
			for (int j = 0; j < answer.length(); j++) {
				if (usedguess[j] || usedanswer[i] || i == j)
					continue;
				if (answer.charAt(i) == guess.charAt(j)) {
					usedguess[j] = usedanswer[i] = true;
					inexact++;
				}
			}
		}

		return "    " + exact + " Exact and " + inexact + " Inexact\n";
	}
}