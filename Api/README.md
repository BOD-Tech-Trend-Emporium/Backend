# Tech-Trend-Emporium Backend

## Project Overview
Tech-Trend-Emporium is an e-commerce platform designed to manage the store’s inventory, customer orders, and payment processing.

## Branch Strategy
This project follows the Git-flow strategy with the following key branches:
- **Main**: Contains the production version of the project.
- **Release**: Serves as a mid step to check if the release is ready to put into production.
- **Develop**: Serves as the integration branch for features.
- **Feature**: Separate branches created for new features.
- **Hotfix**: Separate branches created for make fast ans small changes in production envirorment.

## Contribution Guidelines
All changes to the `main` branch must be done via Pull Requests (PRs) with at least **two approvals** before merging. No direct commits to `main` are allowed.

### Steps to Contribute:
1. Clone the repository:  
    ```bash
    git clone https://github.com/username/tech-trend-emporium.git
    ```
2. Checkout the `develop` branch and create a feature branch:
    ```bash
    git checkout develop
    git checkout -b feature/your-feature-name
    ```
3. Implement your changes and commit them:
    ```bash
    git commit -m "Add your message"
    git push origin feature/your-feature-name
    ```
4. Create a Pull Request (PR) from your feature branch to `develop` and wait for at least two approvals before merging.
5. Merge `develop` into `release` once the feature is stable.
6. Merge `release` into `main` once the team is sure the changes are ready to be push into production envirorment

## Documentation
A detailed documentation on the branching strategy and contribution process is available in the project’s [Wiki](https://github.com/BOD-Tech-Trend-Emporium/backend/wiki).

## Authors
- [Santiago Alvarez Ricardo](https://github.com/salvarezri)
- [Jhoan Oswaldo Ome Vega](https://github.com/Oswe-gif)
- [Jonathan Buitrago Roncancio](https://github.com/jonathanb500)
