import json
import os
import sys
import argparse
from datetime import datetime

SCHEDULE_PATH = "ShopSchedule.json"

def load_schedule():
    if os.path.exists(SCHEDULE_PATH):
        with open(SCHEDULE_PATH, 'r') as f:
            return json.load(f)
    return {"active_packs": {}, "default_min_level": 0, "default_max_purchases": 1, "global_end_utc": 2000000000}

def save_schedule(data):
    with open(SCHEDULE_PATH, 'w') as f:
        json.dump(data, f, indent=2)

def main():
    parser = argparse.ArgumentParser(description="Shop Schedule Admin Tool")
    subparsers = parser.add_subparsers(dest="command")

    # Add command
    add_parser = subparsers.add_parser("add", help="Add or update an offer")
    add_parser.add_argument("pack_id", type=str, help="ID of the pack")
    add_parser.add_argument("--level", type=int, default=0, help="Min level required")
    add_parser.add_argument("--limit", type=int, default=1, help="Max purchases")
    add_parser.add_argument("--start", type=int, default=0, help="Start UTC timestamp")
    add_parser.add_argument("--end", type=int, default=2147483647, help="End UTC timestamp")

    # Remove command
    rem_parser = subparsers.add_parser("remove", help="Remove an offer")
    rem_parser.add_argument("pack_id", type=str)

    # List command
    subparsers.add_parser("list", help="List active offers")

    args = parser.parse_args()
    data = load_schedule()

    if args.command == "add":
        data["active_packs"][args.pack_id] = {
            "min_level": args.level,
            "max_purchases": args.limit,
            "start_utc": args.start,
            "end_utc": args.end
        }
        save_schedule(data)
        print(f"Added/Updated pack {args.pack_id}")
    
    elif args.command == "remove":
        if args.pack_id in data["active_packs"]:
            del data["active_packs"][args.pack_id]
            save_schedule(data)
            print(f"Removed pack {args.pack_id}")
        else:
            print("Pack not found in schedule")

    elif args.command == "list":
        print(f"{'ID':<10} | {'MinLvl':<6} | {'Limit':<6} | {'Start':<12} | {'End':<12}")
        print("-" * 60)
        for pid, cfg in data["active_packs"].items():
            print(f"{pid:<10} | {cfg.get('min_level'):<6} | {cfg.get('max_purchases'):<6} | {cfg.get('start_utc'):<12} | {cfg.get('end_utc'):<12}")

if __name__ == "__main__":
    main()
