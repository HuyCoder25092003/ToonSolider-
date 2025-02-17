﻿#region copyright
// ------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// ------------------------------------------------------
#endregion

namespace CodeStage.AntiCheat.ObscuredTypes
{
	using System;
	using System.Numerics;
	using UnityEngine;
	using Utils;
	using Detectors;

	/// <summary>
	/// Use it instead of regular <c>BigInteger</c> for any cheating-sensitive variables.
	/// </summary>
	/// <strong><em>Regular type is faster and memory wiser comparing to the obscured one!</em></strong><br/>
	/// Feel free to use regular types for all short-term operations and calculations while keeping obscured type only at the long-term declaration (i.e. class field).
	[Serializable]
	public partial struct ObscuredBigInteger : IObscuredType
	{
		[SerializeField] internal SerializableBigInteger hiddenValue;
		[SerializeField] internal SerializableBigInteger fakeValue;
		[SerializeField] internal uint currentCryptoKey;
		[SerializeField] internal bool fakeValueActive;
		[SerializeField] internal bool inited;

		private ObscuredBigInteger(BigInteger value)
		{
			currentCryptoKey = GenerateKey();
			hiddenValue = new SerializableBigInteger
			{
				value = new BigInteger(value.ToByteArray())
			};
			hiddenValue = hiddenValue.Encrypt(currentCryptoKey);

			fakeValue = new SerializableBigInteger();

#if UNITY_EDITOR
			fakeValue.value = value;
			fakeValueActive = true;
#else
			var detectorRunning = ObscuredCheatingDetector.ExistsAndIsRunning;
			fakeValue.value = detectorRunning ? value : 0;
			fakeValueActive = detectorRunning;
#endif
			inited = true;
		}

		/// <summary>
		/// Encrypts passed value using passed key.
		/// </summary>
		/// Key can be generated automatically using GenerateKey().
		/// \sa Decrypt(), GenerateKey()
		public static BigInteger Encrypt(BigInteger value, uint key)
		{
			var input = new SerializableBigInteger { value = new BigInteger(value.ToByteArray()) };
			input = input.Encrypt(key);
			return input;
		}

		/// <summary>
		/// Decrypts passed value you got from Encrypt() using same key.
		/// </summary>
		/// \sa Encrypt()
		public static BigInteger Decrypt(BigInteger value, uint key)
		{
			var input = new SerializableBigInteger { value = new BigInteger(value.ToByteArray()) };
			return input.Decrypt(key);
		}

		/// <summary>
		/// Creates and fills obscured variable with raw encrypted value previously got from GetEncrypted().
		/// </summary>
		/// Literally does same job as SetEncrypted() but makes new instance instead of filling existing one,
		/// making it easier to initialize new variables from saved encrypted values.
		///
		/// <param name="encrypted">Raw encrypted value you got from GetEncrypted().</param>
		/// <param name="key">Encryption key you've got from GetEncrypted().</param>
		/// <returns>New obscured variable initialized from specified encrypted value.</returns>
		/// \sa GetEncrypted(), SetEncrypted()
		public static ObscuredBigInteger FromEncrypted(BigInteger encrypted, uint key)
		{
			var instance = new ObscuredBigInteger();
			instance.SetEncrypted(encrypted, key);
			return instance;
		}

		/// <summary>
		/// Generates random key. Used internally and can be used to generate key for manual Encrypt() calls.
		/// </summary>
		/// <returns>Key suitable for manual Encrypt() calls.</returns>
		public static uint GenerateKey()
		{
			return RandomUtils.GenerateUIntKey();
		}

		/// <summary>
		/// Allows to pick current obscured value as is.
		/// </summary>
		/// <param name="key">Encryption key needed to decrypt returned value.</param>
		/// <returns>Encrypted value as is.</returns>
		/// Use it in conjunction with SetEncrypted().<br/>
		/// Useful for saving data in obscured state.
		/// \sa FromEncrypted(), SetEncrypted()
		public BigInteger GetEncrypted(out uint key)
		{
			if (!inited)
				Init();
			
			key = currentCryptoKey;
			return hiddenValue;
		}

		/// <summary>
		/// Allows to explicitly set current obscured value. Crypto key should be same as when encrypted value was got with GetEncrypted().
		/// </summary>
		/// Use it in conjunction with GetEncrypted().<br/>
		/// Useful for loading data stored in obscured state.
		/// \sa FromEncrypted()
		public void SetEncrypted(BigInteger encrypted, uint key)
		{
			inited = true;
			hiddenValue.value = encrypted;
			currentCryptoKey = key;

			if (ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				fakeValueActive = false;
				fakeValue.value = InternalDecrypt();
				fakeValueActive = true;
			}
			else
			{
				fakeValueActive = false;
			}
		}

		/// <summary>
		/// Alternative to the type cast, use if you wish to get decrypted value
		/// but can't or don't want to use cast to the regular type.
		/// </summary>
		/// <returns>Decrypted value.</returns>
		public BigInteger GetDecrypted()
		{
			return InternalDecrypt();
		}

		public void RandomizeCryptoKey()
		{
			var decrypted = InternalDecrypt();
			currentCryptoKey = GenerateKey();
			hiddenValue.value = decrypted;
			hiddenValue = hiddenValue.Encrypt(currentCryptoKey);
		}

		private BigInteger InternalDecrypt()
		{
			if (!inited)
			{
				Init();
				return 0;
			}

			var decrypted = hiddenValue.Decrypt(currentCryptoKey);

			if (ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && decrypted != fakeValue)
			{
#if ACTK_DETECTION_BACKLOGS
				Debug.LogWarning(ObscuredCheatingDetector.LogPrefix + "Detection backlog:\n" +
				                             $"type: {nameof(ObscuredBigInteger)}\n" +
				                             $"decrypted: {decrypted}\n" +
				                             $"fakeValue: {fakeValue}");
#endif
				ObscuredCheatingDetector.Instance.OnCheatingDetected(this, decrypted, fakeValue);
				return 0;
			}

			return decrypted;
		}
		
		private void Init()
		{
			currentCryptoKey = GenerateKey();
			hiddenValue.value = 0;
			hiddenValue = hiddenValue.Encrypt(currentCryptoKey);
			fakeValue.value = 0;
			fakeValueActive = false;
			inited = true;
		}
	}
}
