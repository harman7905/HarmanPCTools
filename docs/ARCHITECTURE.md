# Architecture

The application is a WPF desktop application.

`MainWindow` handles navigation.

Pages are `UserControl` views.

Services contain system operations such as:
- metrics
- themes
- application settings
- startup entries
- cleanup
- organizer
- gaming launch
- OBS launch
- network diagnostics

User settings are stored under:

`%LOCALAPPDATA%\HarmanPCTools\`

The app has no required cloud backend.
