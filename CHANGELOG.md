# 🚀 Release Notes – Manage School Improvement

Welcome to the ChangeLog (release notes) for **Manage School Improvement**. Here you'll find a summary of key changes, improvements, and fixes over time.
---

## 📅 [v1.5.1] - Q4 – Sprint 6 (Part 1)- 2026-03-20

### 📌 User Stories 

- **265311** - Give users the ability to delete an individual review and rating against an objective
- **265310** - Give users ability to delete objectives
- **266577** - Make tasks read-only when project is "Paused" or "Stopped" (Phase 3 tasks only)
- **266575** - Make tasks read-only when project is "Paused" or "Stopped" (Phase 2 tasks only)
- **258626** - Make tasks read-only when project is "Paused" or "Stopped" (Phase 1 tasks only)
- **265313** - Give users ability to delete progress reviews when no review/ratings have been recorded
- **272499** - Don't allow users to delete Objectives when there are any "Progress recorded" progress reviews
- **272546** - Change button from "Save and return" to "Save and continue"

--

## 📅 [v1.5.0] - Q4 – Sprint 5 (Part 1)- 2026-02-25

### 📌 User Stories 

- **262234** - Changes to "About the school" tab
- **262226** - Delete "Ofsted reports" tab and references to Ofsted judgments from School Summary
- **258618** - Create Statuses and Reasons in back-end
- **258619** - New Project Status Page
- **258621** - Add a Project Status Filter to Project Listing Page
- **258624** - Add "Paused" and "Stopped" Notification Banner
- **258628** - Make Improvement Plan tab read-only when school is "paused" or "stopped"
- **258630** - Add Status History timeline to Project Status tab
- **267694** - Hide project status work before release

### 🐛 Bug Fixes

- **263839** - User is able to add the same school multiple times while navigating back and forth from the browser.

--

## 📅 [v1.4.2] - Q4 – Sprint 3 (Part 1)- 2026-01-30

### 📌 User Stories 

- **258322** - Add postal address to SO contact details check

### 🐛 Bug Fixes

- **259023** - Issue with name validation on confirm supporting org contact page

--

## 📅 [v1.4.1] - Q4 – Sprint 2 (Part 2)- 2026-01-22

### 📌 User Stories 

- **251956** - Add supporting organisation contact details task changes
- **251952** - Add "Federation or education partnership" to Supporting Organisation Types
- **257253** - Remove Supporting Organisation Contact Card from "Choose preferred Supporting Organisation" task

### 🐛 Bug Fixes

- **257417** - Fix cookie issue

--

## 📅 [v1.4.0] - Q4 – Sprint 2 (Part 1)- 2026-01-20

### 📌 User Stories 

- **253097** - Contacts page new design to display existing contacts (+addresses) - School and Supporting Organisation only
- **254373** - Contacts page new design to display existing contacts (+addresses) - Governance bodies
- **254420** - Contacts page new design to display existing contacts (+addresses) - "Contacts without an organisation" section
- **253131** - Add Contact
- **253132** - Edit Contact

--

## 📅 [v1.3.5] - Q3 – Sprint 5 (Part 2)- 2025-12-15

### 📌 User Stories 

- **252516** - Choose Preferred Supporting Organisation task - summary page
- **247168** - Choose preferred Supporting Organisation task - Trust name validation
- **247169** - Choose preferred Supporting Organisation task - School name validation
- **249680** - Choose preferred Supporting Organisation task - Local Authority Validation
- **249958** - Remove "Share email template with adviser" action from "Send introductory email" task

--

## 📅 [v1.3.4] - Q3 – Sprint 5 (Part 1)- 2025-12-04

### 📌 User Stories 

- **240414** - Add Month and Year filters to MSI
- **246492** - Change "Choose preferred Supporting Organisation" task to input Supporting Organisation type
- **246876** - Fix persons API (Contacts)

--

## 📅 [v1.3.3] - Q3 – Sprint 2 (Part 3)- 2025-11-03

### 📌 User Stories 

- **245267** - Change progress against individual objectives to RAG rating
- **245669** - Move overall progress to the end of the add progress review journey and remove radio buttons
--

## 📅 [v1.3.2] - Q3 – Sprint 2 (Part 2)- 2025-11-03

### 📌 User Stories 

- **243896** - Store all SharePoint links in database and package it up for reuse

### 🐛 Bug Fixes

- **243632** - Accessibility issue: Landmarks issue with Banner on all the pages

--

## 📅 [v1.3.1] - Q3 – Sprint 2 (Part 1)- 2025-10-29

### 📌 User Stories 

- **243127** - Highlight all date fields when "date must be today or in the past" validation is triggered
- **238328** - Highlight text field when "Enter school name or URN" error triggered on Select school page
- **238345** - Update content and correct highlighted sections in Allocate an adviser empty name field error summary and message
- **248348** - Highlight text field on case study candidates page and change content when no details entered error triggered

### 🐛 Bug Fixes

- **239821** - Text field inputs with trailing whitespace not trimmed, causing incorrect validation messages

--

## 📅 [v1.3.0] - Q3 – Sprint 1 (Part 2)- 2025-10-17

### 📌 User Stories 

- **224078** - Iterate "Check potential adviser conflicts of interest" task to better reflect the process
- **224092** - Iterate "Record responsible body's response" task
- **223884** - Reorder the "Begin the project" set of tasks to align with RISE Guidance
- **209081** - Rebranding: GOV.UK rebrand and DfE header changes

### 🐛 Bug Fixes

- **218639** - Lexical issue: Success message shows 'schools' instead of school when only 1 record returned
- **233921** - Accessibility issue: Landmarks issue with Back link  on all the pages

--

## 📅 [v1.2.1] - Q3 – Sprint 1 (Part 1) - 2025-10-10

### 📌 User Stories

- **240404** - Add "Cohort" field to MSI database
- **239965** - Adjust the validation for "area of improvement" within the "Enter school improvement plan objectives" task

--

## 📅 [v1.2.0] - Q2 – Sprint 7 - 2025-10-07

### 📌 User Stories (Mandation)

- **238850** - Remove the "Change" links from the engagement concern summary card
- **228478** - Include checklist in the escalate to mandation journey
- **236378** - Understand technically exactly how automating contacts would work and where we get all data points from
- **236193** - Mark engagement concerns as resolved
- **234934** - Flag when an IEB has been issued
- **236189** - Add multiple instances of engagement concerns
- **237598** - Add engagement concern summary to concern flow
- **237018** - Iterate the IEB and Information Powers journeys so they are tied to a particular Engagement Concern
- **238396** - Update the Information Powers feature to the new design
- **238401** - Update the Interim Executive Board feature to the new design

### 🐛 Bug Fixes

- **238906** - Trailing whitespace on email and name inputs causing incorrect validation message
- **235656** - Monitoring: Overall progress fields are not displaying any value (eg.EMPTY) if it's not recorded

--

## 📅 [v1.1.1] - Q2 – Sprint 5 (Part 3) - 2025-09-09

### 🐛 Bug Fixes

- **234569** - Monitoring: Ensure the error messages in the Error Summary appear in the correct order.

--

## 📅 [v1.1.0] - Q2 – Sprint 5 (Part 2) - 2025-09-08

### 📌 User Stories

- **236096** - Release the Escalate to mandation feature

--

## 📅 [v1.0.0] - Q2 – Sprint 5 (Part 1) - 2025-09-04

### 📌 User Stories

- **225330** - Monitoring- View or record progress against objectives
- **232183** - Monitoring- make it mandatory to complete both the progress and details fields when reviewing progress towards objectives
- **231322** - Monitoring- Record and view overall school progress
- **224088** - Add new "Make initial contact with the responsible body" task
- **224071** - Update "Contact the responsible body" task to "Send the formal notification"

### 🐛 Bug Fixes

- **234632** - H2 heading font size not very distinct from paragraph text

--

## 📅 [v0.15.0] - Q2 – Sprint 4 (Part 1) - 2025-08-18

### 📌 User Stories

- **208032** - Enter funding history task green button should say "Continue"

### 🐛 Bug Fixes

- **232291** - Success message is not displaying when user Change Information powers

--

## 📅 [v0.14.1] - Q2 – Sprint 3 (Part 2) - 2025-08-12

### 📌 User Stories

- **227592** - Link to new accessibility statement
- **224026** - Make it clearer how to edit entries into the Engagement Concern tab (Change links) 

### 🐛 Bug Fixes

- **220681** - Notes: Back link missing as well as Error message text is not displaying next to notes text box

--

## 📅 [v0.14.0] - Q2 – Sprint 3 (Part 1) - 2025-08-06

### 📌 User Stories

- **224555** - Changes to 'Allocate an advisor task' following addition of AD group for advisors
- **221581** - For academies: Add the trust the academy belongs to in "About the school" 
- **227590** - Update RISE advisor filter to filter by advisors name
- **225820** - (Monitoring) Build new "Improvement plan" tab //behind the feature flag atm
- **225777** - (Monitoring) New "Enter improvement plan objectives" task //behind the feature flag atm

--

## 📅 [v0.13.1] - Q2 – Sprint 2 (Part 2) - 2025-07-29

### 📌 User Stories

- **224023** - Make it clearer what "Reserves exceed funding level" refers to

--

## 📅 [v0.13.0] - Q2 – Sprint 2 (Part 1) - 2025-07-24

### 📌 User Stories

- **206007** - Filter by: RISE Adviser (part 1/hidden on Test/Prod)
- **221580** -  Filter by: Trust

--

## 📅 [v0.12.1] - Q2 – Sprint 1 (Part 2) - 2025-07-16

### 📌 User Stories

- **223728** - Update 'Arrange adviser's initial visit' task
- **219435** - Remove the "Write and save the Note of Visit" task
- **219353** - Adjust "Record matching decision" task to explicitly reference Assessment Tool 1, accommodate 3rd outcome and record why a school couldn't be reviewed

--

## 📅 [v0.12.0] - Q2 – Sprint 1 (Part 1) - 2025-07-10

### 📌 User Stories

- **221257** - Maintain user scroll position
- **219334** - Adjust "Complete and save the assessment template" to include explicit references to Assessment Tool 1
- **219832** - Change link behaviour for Escalate an Engagement concern

### 🐛 Bug Fixes

- **223413** - Record the responsible body's response task name and title is not displaying correct

---

## 📅 [v0.11.2] - Q1 – Sprint 6 (Part 3) - 2025-07-01

### 🐛 Bug Fixes

- **220026** - Record Engagement concern: Error message(text) is not displaying

---

## 📅 [v0.11.1] - Q1 – Sprint 6 (Part 2) - 2025-06-30

### 📌 User Stories

- **217166** - Merge first 2 checkboxes in the Review the improvement plan task
- **218994** - Users are instructed to complete assessment tool 2 when working on "Choose preferred supporting organisation" task
- **218666** - Provide indicative funding band to the school
- **219833** - Improvement: Do not display a success message when "Save" is clicked without any changes.
- **218698** - Record final funding band

--

## 📅 [v0.11.0] - Q1 – Sprint 6 (Part 1) - 2025-06-20

### 📌 User Stories

- **218224** - Record use of Information Powers
- **218180** - Update Engagement Concern tab to display either engagement concern or mandation
- **217584** - Iteration 1 for recording Escalate an engagement concern
- **217161** - Rename record the school's response and change acceptance language

---

## 📅 [v0.10.2] - Q1 – Sprint 5 (Part 3) - 2025-06-16

### 🐛 Bug Fixes

- **210538** - Text In Tasklist Tags Line-breaks to Right-aligned Instead of Left-aligned

---

## 📅 [v0.10.1] - Q1 – Sprint 5 (Part 2) - 2025-06-12

### 🐛 Bug Fixes

- **211522** - NVDA Reads Out Task Statuses Twice

---

## 📅 [v0.10.0] - Q1 – Sprint 5 (Part 1) - 2025-06-06

### 📌 User Stories

- **217586** - Add 'Assigned to' filter

---

## 📅 [v0.9.0] - Q1 – Sprint 4 (Part 2) - 2025-06-02

### 📌 User Stories

- **206465** - Update page title to match content and URL on school name page
- **215296** - Add the name of the RISE phase to the tasklist
- **215650** - Adding the 'school type' to 'about the school' section
- **217032** - Arrange first face-to-face meeting

### 🐛 Bug Fixes
- **212155** - Very Minor/Enhancement: Cancel links On Engagement concerns & Case study Lack govuk-link classes

---
## 📅 [v0.8.0] - Q1 – Sprint 4 (Part 1) - 2025-05-23

### 📌 User Stories

- **214508** - Add the link to the funding history spreadsheet

---

## 📅 [v0.7.0] - Q1 – Sprint 3 (Part 2 - Previous OFSTED Report Stuff, Cookie Security enhancements, Styling fixes) - 2025-05-20

### 📌 User Stories

- **214995** - Add previous Ofsted report to Ofsted Tab

---

## 📅 [v0.6.0] - Q1 – Sprint 3 (Part 1 - All committed dev/test work) - 2025-05-09

### 📌 User Stories

- **207913** - Updates to 'Review the improvement plan' task
- **207916** - Updates to the 'Request improvement grant offer letter' task
- **210368** - Updates to 'Carry out due diligence on preferred supporting organisation' task

### 🐛 Bug Fixes

- **211680** - Date of Last Inspection Differs On Project Details Page Before Project Create And Also Differs On Ofsted tab from About project tab

---

## 📅 [v0.5.0] - Q1 – Sprint 2 (Part 2) - 2025-05-01

### 📌 User Stories
- **212253** - DEV: Connect AppInsights In Production

---

## 📅 [v0.4.0 - Public Beta] - Q1 – Sprint 2 (Part 1) - 2025-04-30

### 📌 User Stories
- **210784** - DEV: How might we show that a school is not engaging?
- **210785** - DEV: Record which schools are good candidates for case studies
- **210786** - DEV: Updates to 'Request planning grant offer letter task'
- **211017** - DEV: Remove a checkbox from 'Carry out due diligence on preferred supporting organisation' task

---

## 📅 [v0.3.0] - Q1 – Sprint 1 - 2025-04-23

### 📌 User Stories
- **207044** - DEV: Entire row is selectable on the task list
- **205990** - DEV: Improvement to filters on list of schools page - (Copy changes and "Apply filters" CTA button moved from top to bottom)
- **205975** - DEV: Notes time and date format follows GOV style
- **205993** - DEV: Change link improvements and success notification banner in add a school journey
- **206601** - Dev: Task titles on task list respond to zoom and screen size alterations
- **206772** - DEV: Add success notification to confirm filters have been applied
- **206773** - DEV: Add success notification banner when a task is updated 

### 🐛 Bug Fixes
- **206283** – Footer width change

---

## 📅 [v0.2.0] - Q4 – Sprint 7 - 2025-04-06

### 📌 User Stories
- **207641** – Removed heading references from the accessibility statement (user story).

### 🐛 Bug Fixes
- **206285** – Aligned sub-navigation implementation with the GOV.UK Design System for consistency and usability.
- **206774** – Fixed issue where task status tags were not being read out by screen readers.
- **206775** – Resolved issue where *Edit note* text was being read too quickly by screen readers.

---

## 📅 [v0.1.0 – Private Beta] - Q4 - Sprint 6 – 2025-04-01

🎉 **Initial Private Beta release** of Manage School Improvement. We’re excited to open things up for wider use and feedback.