		public void BatchWalaFunction()
		{
			Console.WriteLine("");
			OracleInsertFuncation("Batch Query");

		}

		public void OracleInsertFuncation(string _OracleParameters, int _RepeatCount = 0)
		{
			string OracleParameters = _OracleParameters;
			int RepeatCount = _RepeatCount;

			try
			{

			}
			catch (Exception ex)
			{
				RepeatCount++;
				if (RepeatCount <= 2)
				{
					OracleInsertFuncation(OracleParameters, RepeatCount);

				}
			}

		}
