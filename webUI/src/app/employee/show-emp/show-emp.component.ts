import { Component, OnInit } from '@angular/core';
import { ShareServiceService } from 'src/app/share-service.service';

@Component({
  selector: 'app-show-emp',
  templateUrl: './show-emp.component.html',
  styleUrls: ['./show-emp.component.css'],
})
export class ShowEmpComponent implements OnInit {
  ActivateAddEditEmpComp: boolean;
  ModalTitle: string;
  emp: { EmployeeId: number; EmployeeName: string; Department: string; DateOfJoining: string; PhotoFileName: string; };
  // emp: { EmployeeId: number; EmployeeName: string; Department: string; DateOfJoining: string; PhotoFileName: string; };
  // ModalTitle: string;
  // ActivateAddEditDepComp: boolean;
  constructor(private service: ShareServiceService) {}

  EmployeeList: any = [];

  ngOnInit(): void {
    this.refreshEmpList();
  }
  addClick() {
    this.emp = {
      EmployeeId: 0,
      EmployeeName: '',
      Department: '',
      DateOfJoining: '',
      PhotoFileName: 'anonymous.png',
    };
    this.ModalTitle = 'Add Employee';
    this.ActivateAddEditEmpComp = true;
  }

  editClick(item) {
    this.emp = item;
    this.ModalTitle = 'Edit Employee';
    this.ActivateAddEditEmpComp = true;
  }

  deleteClick(item) {
    if (confirm('Are you sure?')) {
      this.service.deleteEmployee(item.EmployeeId).subscribe((data) => {
        alert(data.toString());
        this.refreshEmpList();
      });
    }
  }
  closeClick() {
    this.ActivateAddEditEmpComp = false;
    this.refreshEmpList();
  }

  refreshEmpList() {
    this.service.getEmpList().subscribe((data) => {
      this.EmployeeList = data;
    });
  }
}
