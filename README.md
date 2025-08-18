# 📚 Biblioteca ISP Nº20 — Proyecto de Cátedra (2025)

**Carrera:** Técnico Superior en Desarrollo de Software  
**Cátedra:** Programación II  
**Año:** 2025  
**Institución:** Instituto Superior de Profesorado Nº20

> Plataforma académica para la **gestión integral de la biblioteca** del instituto: catálogo, ejemplares, usuarios, préstamos y reportes, con apoyo de **IA** para mejorar la experiencia de carga y búsqueda. ✨

---

## 🎯 Objetivo
Desarrollar un sistema web que permita a bibliotecarios, docentes y alumnos **gestionar y consultar** recursos bibliográficos, controlando disponibilidad y trazabilidad de los préstamos.

---

## 🧩 Alcance (visión general)
- **Catálogo** de títulos y metadatos.
- **Múltiples ejemplares** por título y su estado (disponible, prestado, etc.).
- **Usuarios con roles** y permisos diferenciados.
- **Préstamos y devoluciones** con control de disponibilidad.
- **Reportes e impresión** de listados clave.
- **Asistencias con IA** para sinopsis y búsquedas sobre el catálogo.

> Nota: Las políticas de préstamo (plazos, renovaciones, sanciones, etc.) se definen pedagógicamente en la cátedra y pueden implementarse en iteraciones posteriores.

---

## ✅ Requerimientos funcionales (redacción general)

**1. Gestión de libros (ABM)**
- **1.1** Posibilidad de adjuntar **imagen de portada**.
- **1.2** **IA** sugiere/abona la **sinopsis** al momento de registrar el libro.
- **1.3** Administración de **editoriales**.
- **1.4** Administración de **géneros**.
- **1.5** Administración de **autores**.

**2. Gestión de usuarios y roles**
- Manejo de perfiles con permisos: **bibliotecario**, **docente** y **alumno**.

**3. Circulación**
- **Préstamos** (validando **disponibilidad**) y **devoluciones** (restauran disponibilidad).
- **3.1** Búsqueda asistida por **IA** a partir del **catálogo** y una **consulta** del usuario.

**4. Reportes**
- Visualización e **impresión** de **préstamos** y **devoluciones**.
- **4.1** Listado de **préstamos adeudados**, con **segmentación por períodos de mora** (en meses).

---

## 🏗️ Criterios de calidad (no funcionales sugeridos)
- **Usabilidad:** interfaz simple, clara y accesible.
- **Trazabilidad:** registro histórico de movimientos por ejemplar.
- **Seguridad:** control de acceso por rol y protección de datos personales.
- **Escalabilidad:** diseño preparado para crecer en volumen de títulos/ejemplares.
- **Observabilidad:** logs básicos de acciones críticas (altas, préstamos, devoluciones, impresiones).

---

## 🔖 Convenciones del repositorio
- **Ramas:** `main` (estable) · `dev` (integración) · feature branches por funcionalidad.
- **Commits:** mensajes claros y en imperativo (ej.: `feat: alta de libros`).
- **Issues/Boards:** tareas divididas por iteraciones de cátedra.

---

## 🚀 Cómo contribuir (alumnos)
1. Abrir issue con **descripción** y **criterios de aceptación**.
2. Crear rama: `feature/<nombre-corto>`.
3. Pull Request a `dev` con breve **resumen funcional** y **screenshots** si aplica.
4. Solicitar **code review** a docentes/bibliotecario.

---

## 📌 Notas de implementación (a definir en clase)
- **Tecnologías y stack** (framework web, DB, autenticación).
- **Modelo de datos** (títulos, ejemplares, usuarios, préstamos).
- **Integración de IA** (sinopsis/búsqueda).
- **Políticas de préstamo** (parámetros configurables).

---

## 🧑‍🏫 Créditos
Proyecto educativo de **Programación II (2025)** — ISP Nº20.  
Equipo: Docentes y estudiantes de **3er año** de la carrera TSDS. 💙
