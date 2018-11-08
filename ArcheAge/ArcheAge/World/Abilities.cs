﻿// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see licence.txt in the main folder
using System.Collections.Generic;
using System.Linq;
using ArcheAgeGame.ArcheAge.Network;
using LocalCommons.Const;

namespace ArcheAgeGame.ArcheAge.World
{
	/// <summary>
	/// A character's abilities.
	/// </summary>
	public class Abilities
	{
		private Dictionary<int, Ability> _abilities = new Dictionary<int, Ability>();

		/// <summary>
		/// Returns amount of abilities.
		/// </summary>
		public int Count { get { lock (this._abilities) { return this._abilities.Count; } } }

		/// <summary>
		/// The owner of this ability collection.
		/// </summary>
		public Character Character { get; }

		/// <summary>
		/// Creates a new instance for character.
		/// </summary>
		/// <param name="character"></param>
		public Abilities(Character character)
		{
			this.Character = character;
		}

		/// <summary>
		/// Adds ability without updating the client. Overwrites existing
		/// abilities.
		/// </summary>
		/// <param name="ability"></param>
		public void AddSilent(Ability ability)
		{
			lock (this._abilities)
			{
				this._abilities[ability.Id] = ability;
			}
		}

		/// <summary>
		/// Adds ability and updates client. Overwrites existing abilities.
		/// </summary>
		/// <param name="ability"></param>
		public void Add(Ability ability)
		{
			this.AddSilent(ability);
			Send.ZC_ABILITY_LIST(this.Character);
		}

		/// <summary>
		/// Removes the ability with the given id, returns false if it
		/// didn't exist.
		/// </summary>
		/// <param name="abilityId"></param>
		/// <returns></returns>
		public bool Remove(int abilityId)
		{
			lock (this._abilities)
			{
				return this._abilities.Remove(abilityId);
			}
		}

		/// <summary>
		/// Returns list of all abilities.
		/// </summary>
		/// <returns></returns>
		public Ability[] GetList()
		{
			lock (this._abilities)
			{
				return this._abilities.Values.ToArray();
			}
		}

		/// <summary>
		/// Returns true if character has the ability at at least the
		/// given level.
		/// </summary>
		/// <param name="abilityId"></param>
		/// <returns></returns>
		public bool Has(int abilityId, int level = 0)
		{
			var ability = this.Get(abilityId);
			return (ability != null && ability.Level >= level);
		}

		/// <summary>
		/// Returns the ability with the given id, or null if it didn't
		/// exist.
		/// </summary>
		/// <param name="abilityId"></param>
		/// <returns></returns>
		public Ability Get(int abilityId)
		{
			lock (this._abilities)
			{
				this._abilities.TryGetValue(abilityId, out var ability);
				return ability;
			}
		}

		/// <summary>
		/// Toggles ability with the given class name on or off.
		/// Returns whether it was successfully toggled.
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public bool Toggle(string className)
		{
			var data = ArcheAgeGame.Instance.Data.AbilityDb.Find(className);
			if (data == null || data.Passive)
			{
				return false;
			}

			var ability = this.Get(data.Id);
			if (ability == null)
			{
				return false;
			}

			ability.Active = !ability.Active;

			Send.ZC_OBJECT_PROPERTY(this.Character.Connection, ability);
			Send.ZC_ADDON_MSG(this.Character, AddonMessage.RESET_ABILITY_ACTIVE, "Swordman28");

			return true;
		}
	}
}
