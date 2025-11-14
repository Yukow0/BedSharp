# BedSharp

[![Build](https://img.shields.io/badge/build-unknown-lightgrey)](https://github.com/Yukow0/BedSharp/actions)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet)](https://dotnet.microsoft.com/download/dotnet/9.0)

BedSharp is an experimental, native C# server project for Minecraft Bedrock Edition. This repository contains an early-stage prototype focused on the networking layer: a first-pass RakNet implementation and the beginning of Bedrock protocol handling. If you are looking for a production-ready server, this project is not there yet — it is intended for learning, experimentation, and contribution.

Status
------
- Project state: very early prototype / proof-of-concept
- Implemented (current):
    - Basic RakNet connection handling (initial packet framing and parsing)
    - Early Bedrock protocol packet structures and partial handling
    - Initial client join/login handshake flow (proof that a client can begin the join process)
- Not implemented / TODO:
    - World storage, chunk streaming, entities, physics, or gameplay logic
    - Complete RakNet or Bedrock protocol coverage (many packet types missing)
    - Authentication persistence, player management, or plugin system
    - Hardened networking, security protections, and production readiness

Why this project exists
-----------------------
The goal is to build a clean, C#-first Bedrock server core that can eventually be extended. At this stage the focus is on correctly implementing and testing the low-level networking and protocol bits so future features can be layered on top.

Requirements
------------
- .NET 9.0 SDK
- A Minecraft Bedrock client for testing (recommended: same major version as your protocol definitions)

Quickstart
----------
1. Clone:
   git clone https://github.com/Yukow0/BedSharp.git
   cd BedSharp

2. Build:
   dotnet build

3. Run the server project:
   dotnet run --project BedSharp

Notes:
- This will run the prototype server; expect limited behaviour: incoming connections may reach the join handshake but then disconnect because higher-level features are not implemented.
- Check the console output for RakNet/Bedrock debug logs (where present) to see packet-level activity.

Repository layout (high-level)
------------------------------
- src/
    - BedSharp (server entry / small host used for testing)
    - Protocols/
        - RakNet — initial RakNet framing / parsing code
        - Bedrock — packet definitions and partial handlers
- tests/ — protocol unit tests (if present)
- docs/ — notes and experiments (if present)

Development notes
-----------------
- Protocol work aims to be explicit and well-tested. Prefer small, focused unit tests for packet parsing/serialization.
- Keep public APIs minimal during the protocol stabilization phase; refactor for ergonomics after protocol coverage is stable.
- Use defensive parsing and logging to help diagnose mismatched client versions or malformed packets.

How you can help
----------------
- Try the prototype and open issues describing failures, logs, and client versions.
- Contribute tests for packet parsing/serialization.
- Implement missing packet handlers or extend the RakNet reliability layer.
- Help define a small, stable "join" test-suite that reproduces a full client login sequence.

Roadmap (short, prioritized)
---------------------------
1. Complete RakNet reliability/ordering handling for the packets used during client join.
2. Add remaining Bedrock packets needed to complete a full join and basic keepalive.
3. Implement a minimal in-memory player/session manager so connected clients can be tracked.
4. Add simple configuration and logging.
5. Begin work on world data streaming (chunks) and basic entity stubs.

Security & Support
------------------
- This project is experimental. Do not expose it to untrusted networks or use for production.
- If you find a security issue, open a private report or create a GitHub issue with "security" in the title and sufficient reproduction details.

Contributing
------------
Contributions are welcome. Suggested workflow:
1. Fork the repository.
2. Create a branch for your work: git checkout -b feature/short-description
3. Add tests for protocol behaviour where possible.
4. Open a pull request with a clear description and relevant logs/tests.

License
-------
MIT — see LICENSE file.

Contact
-------
Maintainer: Yukow0 — https://github.com/Yukow0
