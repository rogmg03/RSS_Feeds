# RSS Feeds Application - React Migration Prompt for Lovable

## Application Overview

Build a React frontend application for **RSS Feeds**, a personal RSS feed aggregator that allows users to:
- Follow/unfollow RSS feeds
- View a personalized timeline of articles from followed feeds
- Save ("like") articles for later reading
- Discover new RSS feeds to follow
- View their saved articles collection

The application has a **minimal, modern UI** with Spanish language support and a clean, curated design aesthetic.

## Backend API

The React app should connect to an existing ASP.NET Core API service locator at:
- **Base URL**: `https://localhost:7094/api/ServiceLocator`

### Authentication Endpoints

**POST** `/api/ServiceLocator/auth/login`
- Request Body:
  ```json
  {
    "email": "user@example.com",
    "password": "password123"
  }
  ```
- Response (200 OK):
  ```json
  {
    "id": 1,
    "email": "user@example.com",
    "nombre": "User Name"
  }
  ```
- Error: 401 Unauthorized if credentials are invalid

**POST** `/api/ServiceLocator/auth/register`
- Request Body:
  ```json
  {
    "email": "user@example.com",
    "password": "password123",
    "nombre": "User Name"
  }
  ```
- Response: `true` (boolean) on success
- Error: 400/409 if email already exists

### Feed Endpoints

**GET** `/api/ServiceLocator/feeds`
- Returns: Array of `FeedDTO`
  ```json
  [
    {
      "id": 1,
      "url": "https://example.com/feed.xml",
      "titulo": "Feed Title",
      "descripcion": "Feed description",
      "idioma": "es",
      "categoria": "Technology",
      "ultimaLectura": "2024-01-01T00:00:00Z"
    }
  ]
  ```

### User Feed Subscriptions

**GET** `/api/ServiceLocator/usuario-feed`
- Returns: Array of `UsuarioFeedDTO`
  ```json
  [
    {
      "id": 1,
      "usuarioId": 1,
      "feedId": 5,
      "alias": "My Custom Name",
      "creadoEn": "2024-01-01T00:00:00Z"
    }
  ]
  ```

**POST** `/api/ServiceLocator/usuario-feed`
- Request Body:
  ```json
  {
    "usuarioId": 1,
    "feedId": 5,
    "alias": "Optional Custom Name"
  }
  ```
- Response: `true` (boolean)

**DELETE** `/api/ServiceLocator/usuario-feed/{id}`
- Unfollows a feed (removes subscription)

### Articles Endpoints

**GET** `/api/ServiceLocator/articulos`
- Returns: Array of `ArticuloDTO`
  ```json
  [
    {
      "id": 1,
      "feedId": 5,
      "titulo": "Article Title",
      "link": "https://example.com/article",
      "descripcion": "Article description",
      "contenido": "Full article content",
      "autor": "Author Name",
      "imagen": "https://example.com/image.jpg",
      "fechaPublicacion": "2024-01-01T00:00:00Z",
      "creadoEn": "2024-01-01T00:00:00Z"
    }
  ]
  ```

**GET** `/api/ServiceLocator/articulos/feed/{feedId}`
- Returns articles for a specific feed

**POST** `/api/ServiceLocator/articulos`
- Request Body: Same as `ArticuloDTO` above (without `id`)
- Response: Created article ID (number)

### Saved Articles (Likes)

**GET** `/api/ServiceLocator/usuario-articulos-guardados`
- Returns: Array of `UsuarioArticulosGuardadoDTO`
  ```json
  [
    {
      "id": 1,
      "usuarioId": 1,
      "articuloId": 10,
      "fechaGuardado": "2024-01-01T00:00:00Z",
      "notas": "Optional notes"
    }
  ]
  ```

**POST** `/api/ServiceLocator/usuario-articulos-guardados`
- Request Body:
  ```json
  {
    "usuarioId": 1,
    "articuloId": 10,
    "notas": null
  }
  ```
- Response: Created ID (number)

**DELETE** `/api/ServiceLocator/usuario-articulos-guardados/{id}`
- Removes a saved article (unlike)

## Application Pages & Features

### 1. Authentication Pages

#### Login Page (`/login`)
- Split-screen layout with:
  - **Left side (Hero)**: Branding panel with:
    - Brand mark "RS" and "RSS Feeds - Curated Stream"
    - Hero title: "Tu timeline de conocimiento."
    - Subtitle explaining the app
    - Feature bullets:
      - "Descubre feeds por tema y popularidad"
      - "Guarda artículos con un 'like'"
      - "Control total: follow / unfollow en un toque"
    - Pills: "Privado", "Rápido", "Minimal"
  - **Right side (Form)**: Login form with:
    - Email input (with mail icon)
    - Password input (with lock icon, toggle visibility)
    - Error messages displayed if login fails
    - "Entrar" (Enter) button
    - Link to register page

#### Register Page (`/register`)
- Similar split-screen layout
- Form fields:
  - Email
  - Nombre (Name)
  - Password
  - Confirm Password
- "Crear cuenta" (Create account) button
- Link to login page

### 2. Main Application (Protected Routes)

#### Layout Structure
- **Sidebar** (fixed left):
  - Brand section at top: "RS" mark + "RSS Feeds - Curated Stream"
  - Navigation:
    - 🏠 Home
    - 🔍 Discover
    - ❤️ Saved
    - ⎋ Logout
- **Topbar** (fixed top):
  - Search box placeholder: "Buscar feeds, temas o fuentes…"
  - User info: Name and Email
- **Main Content Area**: Scrollable page content

#### Home/Timeline Page (`/`)
- Shows articles from all followed feeds, sorted by publication date (newest first)
- Each article card displays:
  - Article image (if available)
  - Feed source name
  - Publication date (formatted: "dd MMM yyyy")
  - Article title
  - Article description (truncated if long)
  - Actions:
    - Like button (🤍 when not liked, ❤️ when liked)
    - "Leer" (Read) button that opens article link in new tab
- Empty state if no feeds followed:
  - Illustration: 📰
  - Title: "Tu feed está vacío"
  - Message about following feeds
  - Button: "Buscar feeds"

**Important**: Articles are fetched from RSS feeds in real-time. The backend reads RSS feeds and combines them with saved articles from the database. You'll need to integrate RSS reading on the frontend OR the timeline should show articles from the API (which already handles RSS reading server-side).

#### Discover Page (`/discover`)
- Page header:
  - Title: "Descubrir feeds"
  - Subtitle: "Explora fuentes y empieza a construir tu timeline personal."
- Search box: "Buscar feeds por nombre o tema…"
- Grid of feed cards, each showing:
  - Feed avatar (first 2 letters of title, uppercase)
  - Feed title
  - Feed URL
  - Feed description
  - Follow/Following button:
    - "Seguir" (Follow) if not followed
    - "Siguiendo" (Following) if already followed
- Follow modal (when clicking "Seguir"):
  - Title: "Seguir feed"
  - Feed name display
  - Optional alias input
  - "Seguir" and "Cancelar" buttons

#### Saved Page (`/saved`)
- Shows all articles the user has "liked" (saved)
- Each card same as Home timeline cards
- Empty state if no saved articles:
  - Illustration: ❤️
  - Title: "No tienes artículos guardados"
  - Message about saving articles
  - Button: "Ir al feed"

## Technical Requirements

### State Management
- Use React Context API or state management library for:
  - User authentication state (user ID, email, name)
  - User's followed feeds
  - User's saved articles (for quick like/unlike UI updates)

### API Integration
- Create an API service/utility to handle all API calls
- Store auth token/user session (use localStorage or sessionStorage)
- Include error handling for API failures
- Show loading states during API calls

### Authentication Flow
1. User logs in → store user data in state/localStorage
2. Protected routes check authentication → redirect to `/login` if not authenticated
3. Include user ID in requests for user-specific data
4. Logout clears session and redirects to login

### Key Features Implementation

**Following a Feed:**
1. User clicks "Seguir" on Discover page
2. Modal opens to optionally set alias
3. POST to `/usuario-feed` with `usuarioId`, `feedId`, and optional `alias`
4. Update UI to show "Siguiendo" state
5. Refresh or update feed list

**Unfollowing a Feed:**
1. User clicks "Siguiendo" button
2. DELETE to `/usuario-feed/{id}`
3. Update UI

**Liking an Article:**
1. User clicks like button on article
2. If article doesn't exist in DB:
   - POST article to `/articulos` (create article)
   - Get created article ID
3. POST to `/usuario-articulos-guardados` with `usuarioId` and `articuloId`
4. Update button state to show ❤️

**Unliking an Article:**
1. User clicks liked button (❤️)
2. DELETE to `/usuario-articulos-guardados/{id}`
3. Update button state to show 🤍

### Timeline Article Loading
The timeline should:
1. Get user's followed feeds from `/usuario-feed`
2. Filter to current user's feeds
3. For each followed feed, fetch articles from `/articulos/feed/{feedId}`
4. Combine all articles and sort by `fechaPublicacion` (newest first)
5. Check which articles are already liked by getting `/usuario-articulos-guardados`
6. Display timeline with like status

### Styling & Design
- Modern, minimal design
- Clean typography (Inter font family)
- Subtle shadows and rounded corners
- Responsive design (mobile-friendly)
- Spanish language throughout
- Use emoji icons as in the original design (🏠, 🔍, ❤️, 🤍, ❤️, etc.)
- Color scheme: Neutral with subtle accents
- Card-based layouts for articles and feeds

### Routing
- `/login` - Login page
- `/register` - Register page
- `/` - Home/Timeline (protected)
- `/discover` - Discover feeds (protected)
- `/saved` - Saved articles (protected)

### Additional Considerations
- Handle loading states (skeletons or spinners)
- Show error messages for failed API calls
- Implement search functionality (client-side filtering for now)
- Optimistic UI updates where appropriate (like/unlike)
- Prevent duplicate article saves (check if article already exists before creating)

## Data Models Reference

### FeedDTO
```typescript
interface FeedDTO {
  id: number;
  url: string;
  titulo?: string;
  descripcion?: string;
  idioma?: string;
  categoria?: string;
  ultimaLectura?: string; // ISO date string
}
```

### ArticuloDTO
```typescript
interface ArticuloDTO {
  id: number;
  feedId: number;
  titulo: string;
  link: string;
  descripcion?: string;
  contenido?: string;
  autor?: string;
  imagen?: string;
  fechaPublicacion?: string; // ISO date string
  creadoEn?: string; // ISO date string
}
```

### UsuarioFeedDTO
```typescript
interface UsuarioFeedDTO {
  id: number;
  usuarioId: number;
  feedId: number;
  alias?: string;
  creadoEn?: string; // ISO date string
}
```

### UsuarioArticulosGuardadoDTO
```typescript
interface UsuarioArticulosGuardadoDTO {
  id: number;
  usuarioId: number;
  articuloId: number;
  fechaGuardado?: string; // ISO date string
  notas?: string;
}
```

### LoginResponseDTO
```typescript
interface LoginResponseDTO {
  id: number;
  email?: string;
  nombre?: string;
}
```

## Development Notes

- The backend API is already running and functional
- All API endpoints use JSON
- CORS may need to be configured on the backend for React app domain
- Use environment variables for API base URL
- Consider using React Query or SWR for data fetching and caching
- Use React Router for routing
- Implement proper TypeScript types for all API responses

## Deliverables

A fully functional React application with:
1. Authentication (login/register)
2. Protected routes
3. Timeline/Home page with articles
4. Discover page with feed browsing
5. Saved articles page
6. Like/unlike functionality
7. Follow/unfollow functionality
8. Modern, responsive UI matching the design aesthetic
9. Spanish language interface
10. Error handling and loading states

