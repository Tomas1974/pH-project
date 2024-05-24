import {Component, OnDestroy, OnInit} from '@angular/core';
import {DataService} from "../../Services/Data.service";

@Component({
  selector: 'app-Log',
  templateUrl: './log.page.html',
  styleUrls: ['./log.page.scss'],
})

export class LogPage implements OnInit, OnDestroy {

  statusEntries: any[] = [];

  constructor(public dataService : DataService) {}

  ngOnInit(): void {

    this.dataService.getStatusFromServer();
    this.statusEntries = this.dataService.getStatusArray();
  }

  ngOnDestroy(): void {}

}


