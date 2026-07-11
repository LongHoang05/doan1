---
name: Serene Boutique
colors:
  surface: '#fbf9f9'
  surface-dim: '#dbdad9'
  surface-bright: '#fbf9f9'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f5f3f3'
  surface-container: '#efeded'
  surface-container-high: '#e9e8e7'
  surface-container-highest: '#e3e2e2'
  on-surface: '#1b1c1c'
  on-surface-variant: '#4e4445'
  inverse-surface: '#303031'
  inverse-on-surface: '#f2f0f0'
  outline: '#807475'
  outline-variant: '#d2c3c4'
  surface-tint: '#70585c'
  primary: '#70585c'
  on-primary: '#ffffff'
  primary-container: '#f5d6da'
  on-primary-container: '#735b5f'
  inverse-primary: '#ddbfc3'
  secondary: '#5f5e5e'
  on-secondary: '#ffffff'
  secondary-container: '#e5e2e1'
  on-secondary-container: '#656464'
  tertiary: '#5d5f5f'
  on-tertiary: '#ffffff'
  tertiary-container: '#dddddd'
  on-tertiary-container: '#606161'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#fadbdf'
  primary-fixed-dim: '#ddbfc3'
  on-primary-fixed: '#28171a'
  on-primary-fixed-variant: '#574145'
  secondary-fixed: '#e5e2e1'
  secondary-fixed-dim: '#c8c6c5'
  on-secondary-fixed: '#1c1b1b'
  on-secondary-fixed-variant: '#474646'
  tertiary-fixed: '#e2e2e2'
  tertiary-fixed-dim: '#c6c6c7'
  on-tertiary-fixed: '#1a1c1c'
  on-tertiary-fixed-variant: '#454747'
  background: '#fbf9f9'
  on-background: '#1b1c1c'
  surface-variant: '#e3e2e2'
typography:
  headline-lg:
    fontFamily: Be Vietnam Pro
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
    letterSpacing: -0.02em
  headline-md:
    fontFamily: Be Vietnam Pro
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
    letterSpacing: -0.01em
  body-lg:
    fontFamily: Be Vietnam Pro
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-md:
    fontFamily: Be Vietnam Pro
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  label-lg:
    fontFamily: Be Vietnam Pro
    fontSize: 16px
    fontWeight: '500'
    lineHeight: 24px
  label-md:
    fontFamily: Be Vietnam Pro
    fontSize: 14px
    fontWeight: '500'
    lineHeight: 20px
  headline-lg-mobile:
    fontFamily: Be Vietnam Pro
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  unit: 4px
  gutter: 16px
  margin-mobile: 20px
  margin-desktop: 40px
  stack-sm: 8px
  stack-md: 16px
  stack-lg: 24px
---

## Brand & Style

The design system is rooted in **Modern Minimalism** with a focus on sophisticated retail and lifestyle experiences. It leverages a clean, airy aesthetic that prioritizes content and ease of interaction. The personality is professional yet approachable, characterized by a soft color palette and generous whitespace.

This system evokes a sense of calm and clarity, making it ideal for high-end commerce or creative services. It avoids visual clutter, favoring precise alignment and subtle tonal shifts over aggressive depth or ornamentation.

## Colors

The palette is anchored by a pure white background to ensure a "breathable" interface. 
- **Primary:** A soft, pastel pink (#F5D6DA) used exclusively for primary calls-to-action and key brand moments.
- **Secondary:** A deep black (#111111) for high-impact buttons (like social logins) and primary headings.
- **Tertiary:** A light, warm gray (#F4F4F4) for secondary buttons and container fills.
- **Neutral:** Mid-tone grays for body text, placeholders, and subtle borders.

The contrast ratios are carefully balanced to maintain a premium feel while ensuring legibility. Success and error states should utilize muted versions of green and red to stay consistent with the pastel primary tone.

## Typography

The design system utilizes **Be Vietnam Pro** for all roles. This typeface offers a contemporary, clean look with excellent legibility and a friendly, geometric construction. 

Headlines use a semi-bold weight to establish hierarchy against the white background. Body text is kept at a comfortable 16px for primary reading, while labels and supporting text drop to 14px. Tracking (letter spacing) is slightly tightened for headlines to give them a more "designed" and editorial feel.

## Layout & Spacing

This design system uses a **Fixed Grid** approach for centered content containers (like authentication forms) and a **Fluid Grid** for dashboard or listing views. 

The spacing rhythm is built on a 4px baseline. Components within a card should follow a vertical stack pattern:
- 8px between related labels and inputs.
- 16px between distinct input fields.
- 24px between sections or primary action groups.

On desktop, the central container should not exceed 480px for focused tasks, maintaining a high-end, gallery-like focus. Margins scale from 20px on mobile to a generous 40px+ on larger screens to preserve the minimalist aesthetic.

## Elevation & Depth

Hierarchy is established through **Tonal Layers** and extremely **Ambient Shadows**. 

The main interface background is pure white. Primary containers (cards) use a very soft, high-diffusion shadow (Blur: 40px, Opacity: 4%, Color: #000) to appear subtly lifted. 

Depth levels:
- **Level 0 (Surface):** The main background.
- **Level 1 (Container):** White cards with soft shadows or Tertiary gray backgrounds for footer sections.
- **Level 2 (Interaction):** Subtle inset borders for input fields to indicate "hollow" interactive space.
- **Interactive States:** Buttons do not use heavy shadows; instead, they use slight opacity shifts or subtle darkening of the background color on hover.

## Shapes

The shape language is defined by **Rounded** geometry. Consistent 0.5rem (8px) to 1rem (16px) corner radii are used to soften the professional tone.

- **Standard Elements (Inputs, Small Buttons):** 8px radius.
- **Large Containers (Cards, Modals):** 16px radius (rounded-lg).
- **Icon Wrappers:** Often utilize a circular "Pill" shape for a softer touch.

Borders should be kept thin (1px) and used sparingly, primarily for input fields and separators in a light neutral tone.

## Components

### Buttons
- **Primary:** Background: Pastel Pink (#F5D6DA), Text: White or Primary-Dark. Large padding (12px 24px).
- **Secondary:** Background: Black (#111111), Text: White. Used for secondary high-priority actions.
- **Ghost/Tertiary:** Background: Light Gray (#F4F4F4) or Transparent with underlined text.

### Input Fields
- White background with a 1px #E0E0E0 border.
- Rounded-md (8px).
- Placeholder text in a light neutral gray (#A0A0A0).
- 16px horizontal internal padding.

### Cards
- Pure white background.
- 16px border radius.
- Soft ambient shadow.
- Clear separation of header, body, and footer using tonal backgrounds (Level 1 tertiary gray for footers).

### Chips & Tags
- Used for categories or status.
- Small text (label-md).
- Subtle background fills (Tertiary gray) with 100px border radius.