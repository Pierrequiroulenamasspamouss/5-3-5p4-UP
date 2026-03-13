namespace Kampai.Util
{
	public sealed class FastDoubleToString
	{
		private struct DiyFp
		{
			private const int kDiySignificandSize = 64;

			private const int kDpSignificandSize = 52;

			private const int kDpExponentBias = 1075;

			private const int kDpMinExponent = -1075;

			private const ulong kDpExponentMask = 9218868437227405312uL;

			private const ulong kDpSignificandMask = 4503599627370495uL;

			private const ulong kDpHiddenBit = 4503599627370496uL;

			public ulong f;

			public int e;

			private static readonly ulong[] kCachedPowers_F = new ulong[87]
			{
				18054884314459144840uL, 13451937075301367670uL, 10022474136428063862uL, 14934650266808366570uL, 11127181549972568877uL, 16580792590934885855uL, 12353653155963782858uL, 18408377700990114895uL, 13715310171984221708uL, 10218702384817765436uL,
				15227053142812498563uL, 11345038669416679861uL, 16905424996341287883uL, 12595523146049147757uL, 9384396036005875287uL, 13983839803942852151uL, 10418772551374772303uL, 15525180923007089351uL, 11567161174868858868uL, 17236413322193710309uL,
				12842128665889583758uL, 9568131466127621947uL, 14257626930069360058uL, 10622759856335341974uL, 15829145694278690180uL, 11793632577567316726uL, 17573882009934360870uL, 13093562431584567480uL, 9755464219737475723uL, 14536774485912137811uL,
				10830740992659433045uL, 16139061738043178685uL, 12024538023802026127uL, 17917957937422433684uL, 13349918974505688015uL, 9946464728195732843uL, 14821387422376473014uL, 11042794154864902060uL, 16455045573212060422uL, 12259964326927110867uL,
				18268770466636286478uL, 13611294676837538539uL, 10141204801825835212uL, 15111572745182864684uL, 11258999068426240000uL, 16777216000000000000uL, 12500000000000000000uL, 9313225746154785156uL, 13877787807814456755uL, 10339757656912845936uL,
				15407439555097886824uL, 11479437019748901445uL, 17105694144590052135uL, 12744735289059618216uL, 9495567745759798747uL, 14149498560666738074uL, 10542197943230523224uL, 15709099088952724970uL, 11704190886730495818uL, 17440603504673385349uL,
				12994262207056124023uL, 9681479787123295682uL, 14426529090290212157uL, 10748601772107342003uL, 16016664761464807395uL, 11933345169920330789uL, 17782069995880619868uL, 13248674568444952270uL, 9871031767461413346uL, 14708983551653345445uL,
				10959046745042015199uL, 16330252207878254650uL, 12166986024289022870uL, 18130221999122236476uL, 13508068024458167312uL, 10064294952495520794uL, 14996968138956309548uL, 11173611982879273257uL, 16649979327439178909uL, 12405201291620119593uL,
				9242595204427927429uL, 13772540099066387757uL, 10261342003245940623uL, 15290591125556738113uL, 11392378155556871081uL, 16975966327722178521uL, 12648080533535911531uL
			};

			private static readonly int[] kCachedPowers_E = new int[87]
			{
				-1220, -1193, -1166, -1140, -1113, -1087, -1060, -1034, -1007, -980,
				-954, -927, -901, -874, -847, -821, -794, -768, -741, -715,
				-688, -661, -635, -608, -582, -555, -529, -502, -475, -449,
				-422, -396, -369, -343, -316, -289, -263, -236, -210, -183,
				-157, -130, -103, -77, -50, -24, 3, 30, 56, 83,
				109, 136, 162, 189, 216, 242, 269, 295, 322, 348,
				375, 402, 428, 455, 481, 508, 534, 561, 588, 614,
				641, 667, 694, 720, 747, 774, 800, 827, 853, 880,
				907, 933, 960, 986, 1013, 1039, 1066
			};

			public DiyFp(ulong f, int e)
			{
				this.f = f;
				this.e = e;
			}

			public DiyFp(double d)
			{
				ulong num = (ulong)global::System.BitConverter.DoubleToInt64Bits(d);
				int num2 = (int)((num & 0x7FF0000000000000L) >> 52);
				ulong num3 = num & 0xFFFFFFFFFFFFFL;
				if (num2 != 0)
				{
					f = num3 + 4503599627370496L;
					e = num2 - 1075;
				}
				else
				{
					f = num3;
					e = -1074;
				}
			}

			public global::Kampai.Util.FastDoubleToString.DiyFp Normalize()
			{
				global::Kampai.Util.FastDoubleToString.DiyFp result = this;
				while ((result.f & 0x10000000000000L) == 0L)
				{
					result.f <<= 1;
					result.e--;
				}
				result.f <<= 11;
				result.e -= 11;
				return result;
			}

			public global::Kampai.Util.FastDoubleToString.DiyFp NormalizeBoundary()
			{
				global::Kampai.Util.FastDoubleToString.DiyFp result = this;
				while ((result.f & 0x20000000000000L) == 0L)
				{
					result.f <<= 1;
					result.e--;
				}
				result.f <<= 10;
				result.e -= 10;
				return result;
			}

			public void NormalizedBoundaries(out global::Kampai.Util.FastDoubleToString.DiyFp minus, out global::Kampai.Util.FastDoubleToString.DiyFp plus)
			{
				global::Kampai.Util.FastDoubleToString.DiyFp diyFp = new global::Kampai.Util.FastDoubleToString.DiyFp((f << 1) + 1, e - 1).NormalizeBoundary();
				global::Kampai.Util.FastDoubleToString.DiyFp diyFp2 = ((f != 4503599627370496L) ? new global::Kampai.Util.FastDoubleToString.DiyFp((f << 1) - 1, e - 1) : new global::Kampai.Util.FastDoubleToString.DiyFp((f << 2) - 1, e - 2));
				diyFp2.f <<= diyFp2.e - diyFp.e;
				diyFp2.e = diyFp.e;
				plus = diyFp;
				minus = diyFp2;
			}

			public static global::Kampai.Util.FastDoubleToString.DiyFp GetCachedPower(int e, out int K)
			{
				double num = (double)(-61 - e) * 0.30102999566398114 + 347.0;
				int num2 = (int)global::System.Math.Truncate(num);
				if ((double)num2 != num)
				{
					num2++;
				}
				uint num3 = (uint)((num2 >> 3) + 1);
				K = -(-348 + (int)(num3 << 3));
				return new global::Kampai.Util.FastDoubleToString.DiyFp(kCachedPowers_F[num3], kCachedPowers_E[num3]);
			}

			public static global::Kampai.Util.FastDoubleToString.DiyFp operator -(global::Kampai.Util.FastDoubleToString.DiyFp lhs, global::Kampai.Util.FastDoubleToString.DiyFp rhs)
			{
				return new global::Kampai.Util.FastDoubleToString.DiyFp(lhs.f - rhs.f, lhs.e);
			}

			public static global::Kampai.Util.FastDoubleToString.DiyFp operator *(global::Kampai.Util.FastDoubleToString.DiyFp lhs, global::Kampai.Util.FastDoubleToString.DiyFp rhs)
			{
				ulong num = lhs.f >> 32;
				ulong num2 = lhs.f & 0xFFFFFFFFu;
				ulong num3 = rhs.f >> 32;
				ulong num4 = rhs.f & 0xFFFFFFFFu;
				ulong num5 = num * num3;
				ulong num6 = num2 * num3;
				ulong num7 = num * num4;
				ulong num8 = num2 * num4;
				ulong num9 = (num8 >> 32) + (num7 & 0xFFFFFFFFu) + (num6 & 0xFFFFFFFFu);
				num9 += 2147483648u;
				return new global::Kampai.Util.FastDoubleToString.DiyFp(num5 + (num7 >> 32) + (num6 >> 32) + (num9 >> 32), lhs.e + rhs.e + 64);
			}
		}

		private char[] array = new char[128];

		private static readonly ulong[] kPow10 = new ulong[12]
		{
			1uL, 10uL, 100uL, 1000uL, 10000uL, 100000uL, 1000000uL, 10000000uL, 100000000uL, 1000000000uL,
			10000000000uL, 100000000000uL
		};

		private static readonly char[] cDigitsLut = new char[200]
		{
			'0', '0', '0', '1', '0', '2', '0', '3', '0', '4',
			'0', '5', '0', '6', '0', '7', '0', '8', '0', '9',
			'1', '0', '1', '1', '1', '2', '1', '3', '1', '4',
			'1', '5', '1', '6', '1', '7', '1', '8', '1', '9',
			'2', '0', '2', '1', '2', '2', '2', '3', '2', '4',
			'2', '5', '2', '6', '2', '7', '2', '8', '2', '9',
			'3', '0', '3', '1', '3', '2', '3', '3', '3', '4',
			'3', '5', '3', '6', '3', '7', '3', '8', '3', '9',
			'4', '0', '4', '1', '4', '2', '4', '3', '4', '4',
			'4', '5', '4', '6', '4', '7', '4', '8', '4', '9',
			'5', '0', '5', '1', '5', '2', '5', '3', '5', '4',
			'5', '5', '5', '6', '5', '7', '5', '8', '5', '9',
			'6', '0', '6', '1', '6', '2', '6', '3', '6', '4',
			'6', '5', '6', '6', '6', '7', '6', '8', '6', '9',
			'7', '0', '7', '1', '7', '2', '7', '3', '7', '4',
			'7', '5', '7', '6', '7', '7', '7', '8', '7', '9',
			'8', '0', '8', '1', '8', '2', '8', '3', '8', '4',
			'8', '5', '8', '6', '8', '7', '8', '8', '8', '9',
			'9', '0', '9', '1', '9', '2', '9', '3', '9', '4',
			'9', '5', '9', '6', '9', '7', '9', '8', '9', '9'
		};

		private static void GrisuRound(char[] buffer, int len, ulong delta, ulong rest, ulong ten_kappa, ulong wp_w)
		{
			while (rest < wp_w && delta - rest >= ten_kappa && (rest + ten_kappa < wp_w || wp_w - rest > rest + ten_kappa - wp_w))
			{
				buffer[len - 1] -= '\u0001';
				rest += ten_kappa;
			}
		}

		private static uint CountDecimalDigit32(uint n)
		{
			if (n < 10)
			{
				return 1u;
			}
			if (n < 100)
			{
				return 2u;
			}
			if (n < 1000)
			{
				return 3u;
			}
			if (n < 10000)
			{
				return 4u;
			}
			if (n < 100000)
			{
				return 5u;
			}
			if (n < 1000000)
			{
				return 6u;
			}
			if (n < 10000000)
			{
				return 7u;
			}
			if (n < 100000000)
			{
				return 8u;
			}
			if (n < 1000000000)
			{
				return 9u;
			}
			return 10u;
		}

		private static ulong IntPow(ulong x, uint pow)
		{
			ulong num = 1uL;
			while (pow != 0)
			{
				if ((pow & 1) == 1)
				{
					num *= x;
				}
				x *= x;
				pow >>= 1;
			}
			return num;
		}

		private static ulong GetPowerOf10(int n)
		{
			if (n < kPow10.Length)
			{
				return kPow10[n];
			}
			return IntPow(10uL, (uint)n);
		}

		private static void DigitGen(global::Kampai.Util.FastDoubleToString.DiyFp W, global::Kampai.Util.FastDoubleToString.DiyFp Mp, ulong delta, char[] buffer, int offset, out int len, ref int K)
		{
			global::Kampai.Util.FastDoubleToString.DiyFp diyFp = new global::Kampai.Util.FastDoubleToString.DiyFp((ulong)(1L << -Mp.e), Mp.e);
			global::Kampai.Util.FastDoubleToString.DiyFp diyFp2 = Mp - W;
			uint num = (uint)(Mp.f >> -diyFp.e);
			ulong num2 = Mp.f & (diyFp.f - 1);
			int num3 = (int)CountDecimalDigit32(num);
			len = 0;
			while (num3 > 0)
			{
				uint num4;
				switch (num3)
				{
				case 10:
					num4 = num / 1000000000;
					num %= 1000000000;
					break;
				case 9:
					num4 = num / 100000000;
					num %= 100000000;
					break;
				case 8:
					num4 = num / 10000000;
					num %= 10000000;
					break;
				case 7:
					num4 = num / 1000000;
					num %= 1000000;
					break;
				case 6:
					num4 = num / 100000;
					num %= 100000;
					break;
				case 5:
					num4 = num / 10000;
					num %= 10000;
					break;
				case 4:
					num4 = num / 1000;
					num %= 1000;
					break;
				case 3:
					num4 = num / 100;
					num %= 100;
					break;
				case 2:
					num4 = num / 10;
					num %= 10;
					break;
				case 1:
					num4 = num;
					num = 0u;
					break;
				default:
					num4 = 0u;
					break;
				}
				if (num4 != 0 || len != 0)
				{
					buffer[offset + len++] = (char)(48 + (ushort)num4);
				}
				num3--;
				ulong num5 = ((ulong)num << -diyFp.e) + num2;
				if (num5 <= delta)
				{
					K += num3;
					GrisuRound(buffer, offset + len, delta, num5, GetPowerOf10(num3) << -diyFp.e, diyFp2.f);
					return;
				}
			}
			do
			{
				num2 *= 10;
				delta *= 10;
				char c = (char)(num2 >> -diyFp.e);
				if (c != 0 || len != 0)
				{
					buffer[offset + len++] = (char)(48 + c);
				}
				num2 &= diyFp.f - 1;
				num3--;
			}
			while (num2 >= delta);
			K += num3;
			GrisuRound(buffer, offset + len, delta, num2, diyFp.f, diyFp2.f * GetPowerOf10(-num3));
		}

		private static void Grisu2(double value, char[] buffer, int offset, out int length, out int K)
		{
			global::Kampai.Util.FastDoubleToString.DiyFp diyFp = new global::Kampai.Util.FastDoubleToString.DiyFp(value);
			global::Kampai.Util.FastDoubleToString.DiyFp minus;
			global::Kampai.Util.FastDoubleToString.DiyFp plus;
			diyFp.NormalizedBoundaries(out minus, out plus);
			global::Kampai.Util.FastDoubleToString.DiyFp cachedPower = global::Kampai.Util.FastDoubleToString.DiyFp.GetCachedPower(plus.e, out K);
			global::Kampai.Util.FastDoubleToString.DiyFp w = diyFp.Normalize() * cachedPower;
			global::Kampai.Util.FastDoubleToString.DiyFp mp = plus * cachedPower;
			global::Kampai.Util.FastDoubleToString.DiyFp diyFp2 = minus * cachedPower;
			diyFp2.f++;
			mp.f--;
			DigitGen(w, mp, mp.f - diyFp2.f, buffer, offset, out length, ref K);
		}

		private static void WriteExponent(int K, char[] buffer, ref int len)
		{
			if (K < 0)
			{
				buffer[len++] = '-';
				K = -K;
			}
			if (K >= 100)
			{
				buffer[len++] = (char)(48 + (ushort)(K / 100));
				K %= 100;
				int num = K * 2;
				buffer[len++] = cDigitsLut[num];
				buffer[len++] = cDigitsLut[num + 1];
			}
			else if (K >= 10)
			{
				int num2 = K * 2;
				buffer[len++] = cDigitsLut[num2];
				buffer[len++] = cDigitsLut[num2 + 1];
			}
			else
			{
				buffer[len++] = (char)(48 + (ushort)K);
			}
		}

		private static void MemMove(ref char[] dest, int destOffset, ref char[] source, int sourceOffset, int length)
		{
			for (int num = length - 1; num >= 0; num--)
			{
				dest[destOffset + num] = source[sourceOffset + num];
			}
		}

		private static void Prettify(char[] buffer, int first, ref int length, int k)
		{
			int num = length + k;
			if (length <= num && num <= 21)
			{
				for (int i = length; i < num; i++)
				{
					buffer[first + i] = '0';
				}
				buffer[first + num] = '.';
				buffer[first + num + 1] = '0';
				length = num + 2;
			}
			else if (0 < num && num <= 21)
			{
				MemMove(ref buffer, first + num + 1, ref buffer, first + num, length - num);
				buffer[first + num] = '.';
				length++;
			}
			else if (-6 < num && num <= 0)
			{
				int num2 = 2 - num;
				MemMove(ref buffer, first + num2, ref buffer, first, length);
				buffer[first] = '0';
				buffer[first + 1] = '.';
				for (int j = 2; j < num2; j++)
				{
					buffer[first + j] = '0';
				}
				length += num2;
			}
			else if (length == 1)
			{
				buffer[first + 1] = 'e';
				length = first + 2;
				WriteExponent(num - 1, buffer, ref length);
				length -= first;
			}
			else
			{
				MemMove(ref buffer, first + 2, ref buffer, first + 1, length - 1);
				buffer[first + 1] = '.';
				buffer[first + length + 1] = 'e';
				length = first + length + 2;
				WriteExponent(num - 1, buffer, ref length);
				length -= first;
			}
		}

		public void ToCharArray(double value, out char[] buffer, out int len)
		{
			buffer = array;
			if (double.IsNaN(value))
			{
				string naNSymbol = global::System.Globalization.NumberFormatInfo.InvariantInfo.NaNSymbol;
				naNSymbol.CopyTo(0, buffer, 0, naNSymbol.Length);
				len = naNSymbol.Length;
				return;
			}
			if (double.IsPositiveInfinity(value))
			{
				string positiveInfinitySymbol = global::System.Globalization.NumberFormatInfo.InvariantInfo.PositiveInfinitySymbol;
				positiveInfinitySymbol.CopyTo(0, buffer, 0, positiveInfinitySymbol.Length);
				len = positiveInfinitySymbol.Length;
				return;
			}
			if (double.IsNegativeInfinity(value))
			{
				string negativeInfinitySymbol = global::System.Globalization.NumberFormatInfo.InvariantInfo.NegativeInfinitySymbol;
				negativeInfinitySymbol.CopyTo(0, buffer, 0, negativeInfinitySymbol.Length);
				len = negativeInfinitySymbol.Length;
				return;
			}
			if (value == 0.0)
			{
				buffer[0] = '0';
				buffer[1] = '.';
				buffer[2] = '0';
				len = 3;
				return;
			}
			int num = 0;
			if (value < 0.0)
			{
				buffer[0] = '-';
				value = 0.0 - value;
				num = 1;
			}
			int length;
			int K;
			Grisu2(value, buffer, num, out length, out K);
			Prettify(buffer, num, ref length, K);
			len = length + num;
		}

		public string ToString(double v)
		{
			char[] buffer;
			int len;
			ToCharArray(v, out buffer, out len);
			return new string(buffer, 0, len);
		}
	}
}
