﻿using System;

namespace ETHTPS.Data.Core.Models.DataUpdater
{
    public sealed class LiveDataUpdaterStatus : IBasicLiveUpdaterStatus, IComparable<UpdaterStatus>, IEquatable<UpdaterStatus>
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static LiveDataUpdaterStatus EMPTY = new();
#pragma warning restore CA2211 // Non-constant fields should not be visible
        private const double _UNRELIABILITY_RATIO_THRESHOLD = 1;
        public string Updater { get; set; }
        public string Status { get; set; }
        public string UpdaterType { get; set; }
        public DateTime? LastSuccessfulRunTime { get; set; }
        public int NumberOfSuccesses { get; set; }
        public int NumberOfFailures { get; set; }
        public bool? Enabled { get; set; }
        public bool IsUnreliable
        {
            get
            {
                if (NumberOfFailures == 0 && NumberOfSuccesses > 0)
                    return false;
                if (NumberOfFailures == 0)
                    return false;
                return (NumberOfSuccesses / NumberOfFailures) < _UNRELIABILITY_RATIO_THRESHOLD;
            }
        }
        public bool IsProbablyDown
        {
            get
            {
                if (LastSuccessfulRunTime == null)
                    return true;

                return (DateTime.Now - LastSuccessfulRunTime.Value).TotalMinutes > 60;
            }
        }
        public int CompareTo(UpdaterStatus other) => Status.CompareTo(other.ToString());
        public bool Equals(UpdaterStatus other) => this == other;
        public static bool operator ==(LiveDataUpdaterStatus? status, UpdaterStatus other) => status?.Status == other.ToString();
        public static bool operator !=(LiveDataUpdaterStatus? status, UpdaterStatus other) => !(status == other);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator <(LiveDataUpdaterStatus left, LiveDataUpdaterStatus right)
        {
            return left is null ? right is not null : left.CompareTo(right) < 0;
        }

        public static bool operator <=(LiveDataUpdaterStatus left, LiveDataUpdaterStatus right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        private int CompareTo(LiveDataUpdaterStatus right)
        {
            throw new NotImplementedException();
        }

        public static bool operator >(LiveDataUpdaterStatus left, LiveDataUpdaterStatus right)
        {
            return left is not null && left.CompareTo(right) > 0;
        }

        public static bool operator >=(LiveDataUpdaterStatus left, LiveDataUpdaterStatus right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }
    }
}
