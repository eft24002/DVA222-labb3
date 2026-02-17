//#define VERBOSE

using System;
using System.Collections;
using System.Collections.Generic;

class Program
{
	static IDictionary<string, int> RefDict = new Dictionary<string, int>();
	static IDictionary<string, int> TestMap = new DVA222_Map.Map<string, int>();

	static void Main(string[] args)
	{

		string key;
		int value;

		// Add
		Verify("d.Add('aa',21)", d => d.Add("aa", 21));
		Verify("d.Add('aa',21)", d => d.Add("aa", 21));
		Verify("d.Add('aa',56)", d => d.Add("aa", 56));
		Verify("d.Add('bb',13) as pair", d => d.Add(new KeyValuePair<string, int>("bb", 13)));
		Verify("d.Add('bb',13) as pair", d => d.Add(new KeyValuePair<string, int>("bb", 13)));
		Verify("d.Add('bb',98) as pair", d => d.Add(new KeyValuePair<string, int>("bb", 98)));
		while (RefDict.Count < 200)
		{
			key = RndKey();
			value = RndValue();
			Verify($"d.Add('{key}',{value})", d => d.Add(key, value));
		}
		while (RefDict.Count < 300)
		{
			key = RndKey();
			value = RndValue();
			Verify($"d.Add('{key}',{value}) as pair", d => d.Add(new KeyValuePair<string, int>(key, value)));
		}

		// Add null-key
		value = RndValue();
		Verify($"d.Add(null,{value})", d => d.Add(null, value));
		Verify($"d.Add(null,{value}) as pair", d => d.Add(new KeyValuePair<string, int>(null, value)));

		// ContainsKey
		Verify("d.Add('AA',99)", d => d.Add("AA", 99));
		Verify("d.ContainsKey('AA')", d => d.ContainsKey("AA"));
		Verify("d.ContainsKey(null)", d => d.ContainsKey(null));
		Verify("d.ContainsKey('??')", d => d.ContainsKey("??"));

		// Keys.CopyTo
		Verify("d.CopyTo(keys,0)", d =>
		{
			string[] keys = new string[d.Count];
			d.Keys.CopyTo(keys, 0);
			Array.Sort(keys);
			return keys.Length > 0 ? string.Join("|", keys) : "nothing";
		});

		// Indexer
		Verify("d['AA'] == 99", d => d["AA"] == 99);
		Verify("d['AA'] == 100", d => d["AA"] == 100);
		Verify("d['AA'] = 98", d => d["AA"] = 98);
		Verify("d['AA'] == 99", d => d["AA"] == 99);
		Verify("d['AA'] == 100", d => d["AA"] == 100);
		Verify("d['AA'] == 100", d => d["AA"] == 98);
		Verify("d[null] == 0", d => d[null] == 0);
		Verify("d[null] = 0", d => d[null] = 0);
		Verify("d['BB'] = 10", d => d["BB"] = 10);

		// Remove
		Verify("d.Remove('??')", d => d.Remove("??"));
		Verify("d.Remove('AA')", d => d.Remove("AA"));
		Verify("d.Remove(null)", d => d.Remove(null));
		Verify("d.Remove('??',99)", d => d.Remove(new KeyValuePair<string, int>("??", 99)));
		Verify("d.Remove(null,99)", d => d.Remove(new KeyValuePair<string, int>(null, 99)));
		Verify("d.Add('AA',99)", d => d.Add("AA", 10));
		Verify("d.Remove('AA',99)", d => d.Remove(new KeyValuePair<string, int>("AA", 99)));
		Verify($"d.Remove('AA',10)", d => d.Remove(new KeyValuePair<string, int>("AA", 10)));

		// Values.CopyTo
		Verify("CopyTo(values, 0)", d =>
		{
			int[] values = new int[d.Count];
			d.Values.CopyTo(values, 0);
			Array.Sort(values);
			ulong hash = FnvBasis;
			foreach (int value in values)
				hash = (FnvPrime * hash) ^ (ulong)value;
			return (FnvPrime * hash) ^ (ulong)d.Count;
		});

		// Contains
		Verify("d.Contains('BB',99)", d => d.Contains(new KeyValuePair<string, int>("BB", 99)));
		Verify("d.Contains('BB',10)", d => d.Contains(new KeyValuePair<string, int>("BB", 10)));
		Verify("d.Contains(null,10)", d => d.Contains(new KeyValuePair<string, int>(null, 10)));

		// TryGetValue
		Verify("d.TryGetValue(null,out value)", d =>
		{
			value = -1;
			bool found = d.TryGetValue(null, out value);
			return (found, value);
		});
		Verify("d.TryGetValue('BB',out value)", d =>
		{
			value = -1;
			bool found = d.TryGetValue("BB", out value);
			return (found, value);
		});
		Verify("d.TryGetValue('??',out value)", d =>
		{
			value = -1;
			bool found = d.TryGetValue("??", out value);
			return (found, value);
		});

		// Count
		Verify($"d.Count", d => d.Count);

		// CopyTo
		KeyValuePair<string, int>[] copy = null;
		Verify("d.CopyTo(copy,0) where copy is null", d => d.CopyTo(copy, 0));
		copy = new KeyValuePair<string, int>[1];
		Verify("d.CopyTo(copy,0) where copy is too small", d => d.CopyTo(copy, 0));
		copy = new KeyValuePair<string, int>[TestMap.Count];
		Verify("d.CopyTo(copy,1)", d => d.CopyTo(copy, 1));
		Verify("d.CopyTo(copy,-1)", d => d.CopyTo(copy, -1));
		Verify("d.CopyTo(copy,0)", d =>
		{
			int nhits = 0;
			d.CopyTo(copy, 0);
			for (int i = 0; i < copy.Length; i++)
				if (d.Contains(new KeyValuePair<string, int>(copy[i].Key, copy[i].Value))) ++nhits;
				else throw new Exception($"{copy[i].Key},{copy[i].Value} missing!");
			return $"{nhits}/{copy.Length}";
		});

		// GetEnumerator
		Verify("d.GetEnumerator()", d =>
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, int> entry in d)
				list.Add($"{entry.Key},{entry.Value}");
			list.Sort();
			return list.Count > 0 ? string.Join("|", list) : "nothing";
		});

		// Clear
		Verify("d.Clear()", d => d.Clear());
		Verify("d.Add('gc',10)", d => d.Add("gc", 10));

		// Final message
		Console.WriteLine("If no mismatches are reported, the test is passed!");
	}

	// for methods that return a result a.k.a. Func
	static void Verify<TResult>(string op, Func<IDictionary<string, int>, TResult> func)
	{
		TResult dRes = default;
		TResult mRes = default;
		Exception dEx = null;
		Exception mEx = null;
		// execute func on map and ref
		try { dRes = func(RefDict); } catch (Exception e) { dEx = e; }
		try { mRes = func(TestMap); } catch (Exception e) { mEx = e; }
		// test exceptions match (if any)
		if (dEx?.GetType() != mEx?.GetType())
			Console.WriteLine($"- {op}: exception mismatch, expected: {dEx?.GetType().Name ?? "\u2205"}, got; {mEx?.GetType().Name ?? "\u2205"}");
		// test return-values match (if no exception)
		else if (dEx == null && mEx == null && !EqualityComparer<TResult>.Default.Equals(dRes, mRes))
			Console.WriteLine($"- {op}: result mismatch, expected: {dRes}, got: {mRes}");
#if VERBOSE
		else
			Console.WriteLine($"+ {op}: results match: {mEx?.GetType().Name ?? mRes.ToString()}");
#endif
		// test count
		if (RefDict.Count != TestMap.Count)
			Console.WriteLine($"- {op}: count mismatch! Expected {RefDict.Count}, got {TestMap.Count}");
	}

	enum ResultStatus { NoResultExpected }

	// for methods that return void a.k.a. Action
	static void Verify(string op, Action<IDictionary<string, int>> action) => Verify<ResultStatus>(op, d => { action(d); return ResultStatus.NoResultExpected; });

	// random generator for keys and values
	static Random rnd = new Random(8277);
	static string RndKey() => $"{(char)rnd.Next(97, 123)}{(char)rnd.Next(97, 123)}"; // generate a random string like "[a-z][a-z]"
	static int RndValue() => rnd.Next(96); // generate a random int in [0,95]"

	// Fowler-Noll-Vo hash constants
	const ulong FnvBasis = 14695981039346656037;
	const ulong FnvPrime = 1099511628211;

}