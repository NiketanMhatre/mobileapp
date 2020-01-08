While this list provides a list of steps to do as a Project lead, it is in no way a substitute for the [Project leading guidelines](https://www.notion.so/toggl/Mobile-Project-Leading-fe1eaca52e46498d8575104bf018287b). You should be familiar with that document as well. This checklist is just a helper.

* [ ] 🧠 Accept that you are now a Project Manager and that you are responsible for the success of this project. *This is your project. There are many like it, but this one is yours*. Take a deep breath and let that sink in.

⚠ This Project Leading checklist assumes that the roles of the Project Lead and the Development Lead, as they are defined in Notion Project Leading Guidelines, are conflated into the same person. In the mobile team, that was the case for the most of the projects. If it is not the case with your project, there are two options: 
* This issue can then either be used by both leads on the project, where each marks only the points that match their own responsibilities, 
* The Development lead can skip the Project Lead's points (or mark them as they are completed as Project Lead completes them). 

Since the checklist was created to help with most of the development administration tasks, only the points belonging to the Project lead are marked with an appropriate label.

After creating this issue, and reading thru the above comments and warnings, feel free to delete everything up to (and including) this sentence. This will make the checklist cleaner.

--- 

### 1. Specification
* [ ] 📃 `[Project Lead]` Make sure that there is a [Notion page]((https://www.notion.so/toggl/ad87bf4fd8114411980a988d38842b35?v=0814a8f583864e1ebaf3c7f29f3443f7)) for your project. If there is none, use the `New` button to create a new page by using `Project template (mobile)` template page. Use the project document template guidelines if you are unsure what to write. Feel free to ask a product manager for help with such docs if needed.
* [ ] 📞 `[Project Lead]` If appropriate, create a Slack channel for the project that will allow you seamless discussions about the project without spamming the regular channels.
* [ ] 💬 `[Project Lead]` In coordination with the teamlead, the product, the designers and the architect, research and write the following for your project:
    * [ ] The goals of the project
    * [ ] Why the project is being done (if not obvious from the Goals)
    * [ ] List of features that the project introduces
    * [ ] A list of required designs for the project
    * [ ] A list of other required resources (if any)
    * [ ] A method of measuring the success of the project
* [ ] 🖼 `[Project Lead]` In coordination with the product and the designers, make sure your project has all the required designs.
    * [ ] Link to the relevant Zeplin pages from your project page
* [ ] 🕒 For each feature, make an estimate of how long the feature takes to get to completion
* [ ] 📚 `[Project Lead]` Before continuing to the infrastructure work, ensure that the specification documents are now complete, correct and unambiguous. If need be, read the specs several times so that things don't fall through the cracks. Ask others to review the specs as well. Discuss all ambiguities or unknowns with the relevant parties until there is none.
    * [ ] 🚧 If the project requires any architectural changes or refactoring, have a  discussion with the architect to avoid going the wrong direction.
    * [ ] ♻ If your project touches the sync algorithm in any way, make sure you discuss  your needs with the Sync Manager.
    * [ ] 🎨 Make sure that all designs your project requires are ready.
* [ ] When all of the above is done, the first section is complete. Move to the next one.

### 2. Infrastructure (GitHub related work)

* [ ] 🙈 Decide on the project emoji that will be used for this project. If at all possible, us the unicode character (such as ❤) and not the `:heart:` variant so that it can be easily recognizable and remain an emoji in all situations.
* [ ] 🌿 Create a project branch: `project/name`, such as `project/widgets`.
* [ ] ✨ Create a new Project on GitHub and name it appropriately
* [ ] 📃 Make sure that the Github projects's description contains:
	* [ ] The link to the Notion project page
	* [ ] The link to the Github project branch
	* [ ] Start and due date (copy from `Toggl Plan`)
* [ ] 🔻 Add this issue to your Project.
* [ ] 🅰 Rename the title of this checklist issue to: `<Emoji> Project: <ProjectName>`
	* example: `🎛 Project: Widgets`.
* [ ] ✅ If all of the previous is done, create the issues that will cover all that needs to be done by the project, based on the feature list from the specs. Take great care to ensure that all the issues are complete, unambiguous and correct. Make sure that there's sufficient information in them so that the work on them can be efficient.
	* Required work for an issue should be small enough so that the required time can be easily estimated and that the required solution would be easy to review. The goal is for the issues to not take longer than several hours. If it takes the whole day (or even more), then consider splitting into smaller parts. Use your best judgement here.
* [ ] 🔳 Make sure the project contains all the necessary columns for the issues (according to what the project lead and all the involved developers find to be most effective).
* [ ] 📎 Make sure that all the related issues are assigned to the created project.
* [ ] 🔀 Order the issues the way you think is **most effective** in your current project team, but do not forget about the **critical path**. If possible, put less important features to the end of the queue, in case something needs to be cut because of the delays.
* [ ] ⌚ Estimate the require time for each issue. 
    * [ ] Double-check those estimates with the rest of the project team to make sure that everyone agrees on them.
* [ ] 🕓 `[Project Lead]` Schedule the project in coordination with the team lead, taking into account all the ongoing projects and the schedule of all participants.

---
* [ ] 🔙 Create a PR from this branch into `develop` as soon as the first issue from the project gets squashed into the project branch. _Unfortunatelly, Github does not allow empty PRs, so we have to wait until there's at least a single change in the diff._
* [ ] 🔻 Assign the above mentioned PR to the Github project.


### 2. As the project starts
* [ ] 💬 `[Project Lead]` Organize and execute a **kickoff meeting**. 
	* [ ] Invite all of the following (regardless of their RSVP)
	* Mandatory participants: You and all the devs that will work on the project. 
	* Optional, but highly recommended participants: Team lead, product manager, designers.
	* Consider whether there's any other person, not tied to a role, but still relevant to the project and invite them as well.
* [ ] 📈 `[Project Lead]` Make sure that the teamlead has assigned your project to the Geekbot for purposes of weekly status reports reminders.

### 3. Weekly responsibilities (check these often during the project)
* [ ] 🔙 Merge `develop` back into the project branch.
	* Since the project branch is protected, you will have to do it through a regular PR, as specified in these steps:
		* Create a branch `project/name-mergeback` from `project/name` 
		* Merge `develop` into `project/name-mergeback`
		* Create a PR from `project/name-mergeback` into `project/name`
	* Remember that the mergeback PR has to be `merged` into the project branch and not `squashed`, so that commits from the `develop` are not lost.
* [ ] 📱 Try to organize the work in such a way so that each week a working test build (containing things already done) can be created so that some of the testing  can be done in parallel. If it does not make sense to have only part of the feature, ignore this point.
* [ ] 📃 `[Project Lead]` Go through the project page on Notion and make sure things are up to date there, especially the status of the project.
* [ ] 💬 Try to organize a meeting with the devs in the project to ensure that you are aware of what the exact status is but also to increase the awareness of the potential blockers. 
* 📃 `[Project Lead]` Twice a week, provide a status report thru the Geekbot's faculties. Geekbot will automatically post the update to the `#mobile-updates` channel. Make sure to link to the status on project's channel as well. 
    * [ ] 📃  Inform everyone about the current state of the project, potential blockers or anything else you find a valuable information.
	* 💬 During the Tuesday weekly meeting, you will provide a similar report.

#### Crisis management
⚠ `[Project Lead]` This section is activated when you realize that the deadline is no longer attainable (regardless of the reasons). However, you should be observant enough to anticipate potential delays ahead of time.

* [ ] Inform the teamlead and the product manager immediatelly

SCENARIO 1 - If the project can be completed partially
* [ ] If there are possible features you can cut from your project, that should be step 1. Carefully weigh down what can be cut and make a decision. If you need help, don't hesitate to contact the teamlead for guidance.
* [ ] When you have decided what to do, inform all the involved parties (the teamlead, the product manager and other stakeholders)

SCENARIO 2 - If cutting does not make sense within the context of the project
* [ ] If the delay seems necessary, discuss it with the teamlead as soon as possible. And in this case `as soon as possible` really means **as soon as possible**.
* [ ] In cooperation with the teamlead, decide how to proceed further.
* [ ] When teamlead blesses the recovery strategy, continue as decided.

### 4. After the project is implemented
* [ ] ✅ `[Project Lead]` Test! Test! Test!
	* Besides you and your project team, feel free to employ all Togglrs to help with testing by reaching out on public channels (not `#general`).
* [ ] 🔙 When you are confident that the feature is behaving correctly and that all the features doing what they should be doing, prepare the project branch for the merge into `develop`. This most likely requires a mergeback and a conflict resolution.
	* [ ] Coordinate with the teamlead to find the right moment for merging the 
project PR into `develop`. The teamlead gives the green light in this case.
* [ ] Merge (not squash) the project PR.
* [ ] 💬 `[Project Lead]` Organize a **retrospective meeting**
	* Participants list is the same as in `kickoff` meeting. Invite them all.
	* Discuss the good, the bad and the ugly of the project. Discuss also the potential future of the project and evaluate your project throught the metrics chosen at the beginning of the project. Make sure that someone from the Product is there because they should be as involved in this as you are.
* [ ] 📃 `[Project Lead]` Write a **report** with [similar details](https://www.notion.so/toggl/Mobile-Project-Leading-fe1eaca52e46498d8575104bf018287b#5bc1cf7781e84c699c6fd642ad4775d9) as in the retrospective meeting.
* [ ] `[Project Lead]` Remind the Team Lead to remove you from the Geekbot project reminders schedule

### 5. Aftermath
* [ ] ⚡ `[Project Lead]` Remember to keep an eye on how the users interact with the new features
* [ ] 📃 `[Project Lead]` Write a follow-up document when you see the influence of the feature on our users
