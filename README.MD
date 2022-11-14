# Backend Services For "Gypoo"

Gypoo is a hotel recommendation and booking web application

# Languages and Tools :toolbox:

- :wrench: C#
- :hammer: ASP.NET core

# API

### Hotel

- GET /api/Hotel
- POST /api/Hotel
- GET /api/Hotel/{id}
- PUT /api/Hotel/addRoomTo/{id}

### Room

- GET /api/Room
- POST /api/Room
- GET /api/Room/getroom/{id}
- PUT /api/Room/{id}

### User

- GET /api/User/{id}
- POST /api/User/register
    - username : string
    - password : string
    - comfirmPassword : string
- POST /api/User/login
    - username : string
    - password : string
- GET /api/User/token

### Upload

- POST /api/upload
    - files : List<file>

## Frontend Repository: https://github.com/SroLyQ/Gypoo_Frontend
