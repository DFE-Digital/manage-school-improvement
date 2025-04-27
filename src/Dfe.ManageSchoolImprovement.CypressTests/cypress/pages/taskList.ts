class TaskList {
    public hasHeader(header: string): this {

        cy.get("h1").contains(header);
    
        return this;
      }

    public deleteSchool(): this {
      cy.contains("Delete school").click({ force: true });
  
      return this;
    }
  }
  
  const taskList = new TaskList();
  
  export default taskList;