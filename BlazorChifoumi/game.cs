using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace BlazorChifoumi {
	public class Game 
	{
		private Timer _timer = new Timer(700);
		private RandomHand _randomHand = new RandomHand();

		public EventHandler Go { get; set; }
		public EventHandler CountDown { get; set; }

		public int MachineScore { get; set; }
		public int HumanScore { get; set; }

		public int Counter { get; private set; } = 3;
		public Hand MachineHand { get; private set; }
		public Hand HumanHand { get; private set; }

		public string Status { get; private set; } = "Ready ?!";

		public Game()
		{
			_timer.Elapsed += TimerOnElapsed;
		}

		private void TimerOnElapsed(object sender, ElapsedEventArgs e)
		{
			if (Counter == 1)
			{
				Status = "MI !";
				_timer.Stop();
				Go?.Invoke(this, null);
				return;
			}

			switch (Counter)
			{
				case 3: Status = "CHI !";
					break;
				case 2:
					Status = "FOU !";
					break;
			}
			Counter--;
			CountDown?.Invoke(this, null);
		}

		public void Start()
		{
			Counter = 3;
			MatchStatus = MatchStatus.None;
			CountDown?.Invoke(this,null);
			_timer.Start();
		}


		public MatchStatus MatchStatus { get; set; } = MatchStatus.None;

		public void Evaluate(Hand humanHand)
		{
			MachineHand = _randomHand.Get();
			HumanHand = humanHand;

			if (HumanHand == MachineHand)
				MatchStatus = MatchStatus.Equality;
			else if ((HumanHand == Hand.pierre && MachineHand == Hand.ciseaux) ||
			         (HumanHand == Hand.feuille && MachineHand == Hand.pierre) ||
			         (HumanHand == Hand.ciseaux && MachineHand == Hand.feuille))
			{
				MatchStatus = MatchStatus.HumanWin;
				HumanScore++;
			}

			else
			{
				MatchStatus = MatchStatus.MachineWin;
				MachineScore++;
			}
		}

		public void Clear()
		{
			Counter = 3;
			MachineHand = Hand.None;
			HumanHand = Hand.None;
			MatchStatus = MatchStatus.None;
			MachineScore = 0;
			HumanScore = 0;
			Status = "READY ?!";
			CountDown?.Invoke(this, null);
		}
	}

	public class RandomHand
	{
		private Random _random = new Random(Guid.NewGuid().GetHashCode());
		public Hand Get() => (Hand) _random.Next(1, 4);
	}

	public enum MatchStatus
	{
		Equality,
		MachineWin,
		HumanWin, 
		None
	}

	public enum Hand {
		pierre = 1,
		ciseaux = 2,
		feuille = 3,
		None = 10
	}
}
