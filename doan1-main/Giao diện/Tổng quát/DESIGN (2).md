---
name: Cinema Admin Vertical
colors:
  surface: '#f8f9fb'
  surface-dim: '#d8dadc'
  surface-bright: '#f8f9fb'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f2f4f6'
  surface-container: '#eceef0'
  surface-container-high: '#e6e8ea'
  surface-container-highest: '#e0e3e5'
  on-surface: '#191c1e'
  on-surface-variant: '#3d4946'
  inverse-surface: '#2d3133'
  inverse-on-surface: '#eff1f3'
  outline: '#6d7a77'
  outline-variant: '#bcc9c5'
  surface-tint: '#006b5f'
  primary: '#00685d'
  on-primary: '#ffffff'
  primary-container: '#008376'
  on-primary-container: '#f4fffb'
  inverse-primary: '#70d8c8'
  secondary: '#4c56af'
  on-secondary: '#ffffff'
  secondary-container: '#959efd'
  on-secondary-container: '#27308a'
  tertiary: '#006b1b'
  on-tertiary: '#ffffff'
  tertiary-container: '#1e862d'
  on-tertiary-container: '#f7fff1'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#8df5e4'
  primary-fixed-dim: '#70d8c8'
  on-primary-fixed: '#00201c'
  on-primary-fixed-variant: '#005048'
  secondary-fixed: '#e0e0ff'
  secondary-fixed-dim: '#bdc2ff'
  on-secondary-fixed: '#000767'
  on-secondary-fixed-variant: '#343d96'
  tertiary-fixed: '#94f990'
  tertiary-fixed-dim: '#78dc77'
  on-tertiary-fixed: '#002204'
  on-tertiary-fixed-variant: '#005313'
  background: '#f8f9fb'
  on-background: '#191c1e'
  surface-variant: '#e0e3e5'
typography:
  display-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 28px
    fontWeight: '700'
    lineHeight: 36px
  headline-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
  headline-sm:
    fontFamily: Plus Jakarta Sans
    fontSize: 16px
    fontWeight: '600'
    lineHeight: 24px
  body-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  label-sm:
    fontFamily: Plus Jakarta Sans
    fontSize: 12px
    fontWeight: '500'
    lineHeight: 16px
    letterSpacing: 0.02em
  stat-value:
    fontFamily: Plus Jakarta Sans
    fontSize: 24px
    fontWeight: '700'
    lineHeight: 32px
  display-lg-mobile:
    fontFamily: Plus Jakarta Sans
    fontSize: 24px
    fontWeight: '700'
    lineHeight: 32px
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  container-max: 1440px
  gutter: 24px
  sidebar-width: 260px
  card-padding: 24px
  stack-sm: 8px
  stack-md: 16px
  stack-lg: 32px
---

## Brand & Style

The design system is engineered for high-density administrative workflows, specifically tailored for media management and operational logistics. It adopts a **Corporate / Modern** aesthetic that prioritizes clarity, data legibility, and structural order. 

The visual language communicates reliability and efficiency through a clean white foundation, precise linework, and a sophisticated teal-centric palette. The target audience—system administrators and operations managers—requires a UI that minimizes cognitive load while maximizing the visibility of key performance indicators (KPIs). The emotional response is one of control, precision, and professional calm.

## Colors

The palette is anchored by a deep teal primary color used for actions and active states. A navy secondary color provides a grounding contrast for complex data visualizations.

- **Primary (#00897B):** Used for primary buttons, active sidebar states, and main data bars.
- **Surface:** The background remains a pure white (#FFFFFF) to ensure maximum contrast, while a soft neutral grey (#F5F7F9) is used for the sidebar and page background to separate content areas.
- **Data Visualizations:** A range of teals and deep blues are used to distinguish chart series without creating visual noise.
- **Semantic Colors:** Traditional green, amber, and red tones are utilized for status indicators (Success, Warning, Overdue) but are slightly desaturated to maintain the professional tone.

## Typography

This design system utilizes **Plus Jakarta Sans** for its modern, clean proportions and exceptional legibility in data-heavy environments. 

The hierarchy is strictly enforced: large bold weights are reserved for page titles and critical numeric values (stats), while medium weights define section headers. Body text uses a standard 14px size to balance information density and readability. Micro-copy and labels use a slightly tracked-out uppercase or medium-weight style to differentiate from interactive text.

## Layout & Spacing

The layout follows a **Fixed Sidebar + Fluid Content** model. The sidebar remains at a constant 260px, while the main content area utilizes a 12-column grid system that scales with the viewport.

A consistent 8px spatial rhythm is used throughout the design system. Dashboard cards are separated by 24px gutters. On mobile devices, the grid collapses to a single column, gutters reduce to 16px, and the sidebar transforms into a hidden drawer menu. Content is grouped into logical "clusters" (e.g., Stats Row, Main Charts, List View) with clear vertical margins to prevent visual clutter.

## Elevation & Depth

Hierarchy in this design system is achieved through **Tonal Layers** and extremely subtle **Ambient Shadows**.

- **Level 0 (Background):** The base canvas uses a light neutral grey (#F5F7F9).
- **Level 1 (Cards/Sidebar):** Main content containers are pure white with a 1px border (#E0E4E8) and a soft, low-opacity shadow (0px 4px 12px rgba(0,0,0,0.03)).
- **Level 2 (Overlays/Modals):** These elements use a more pronounced shadow (0px 12px 32px rgba(0,0,0,0.1)) to indicate a significant change in the user's focus.
- **Active States:** Navigational items use a vertical teal bar on the left edge to indicate focus, rather than heavy drop shadows.

## Shapes

The shape language is **Soft**, balancing the rigidity of a corporate dashboard with a modern feel. 

Standard components like cards, input fields, and buttons utilize a 0.25rem (4px) or 0.5rem (8px) corner radius. Large dashboard cards use the 8px radius for a distinct container feel, while smaller elements like chips and tags use a fully pill-shaped (100px) radius to distinguish them from interactive buttons.

## Components

- **Buttons:** Primary buttons are solid teal with white text. Secondary buttons use a teal outline or a subtle grey ghost style.
- **Input Fields:** Search bars and text inputs use a light grey border that darkens on hover, with a primary teal focus ring.
- **Cards:** White backgrounds with subtle borders. Title areas within cards are separated by hierarchy, not horizontal lines, to keep the UI "airy."
- **Data Visualizations:** Bars in charts should have slightly rounded tops (2px). Pie charts should use the defined chart series colors with a 2px white stroke between segments.
- **Chips/Badges:** Used for status (e.g., "12 Yêu cầu"). These have a light-colored background (tinted from the semantic color) with high-contrast text.
- **Lists:** Data tables use simple dividers (1px #EEE) and generous 16px vertical padding for each row to ensure readability.