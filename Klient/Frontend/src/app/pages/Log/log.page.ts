import {Component, OnDestroy, OnInit} from '@angular/core';
import {DataService} from "../../Services/Data.service";

@Component({
  selector: 'app-Log',
  templateUrl: './log.page.html',
  styleUrls: ['./log.page.scss'],
})

export class LogPage implements OnInit, OnDestroy {

  statusEntries: any[] = [];

  constructor(private dataService : DataService) {}

  ngOnInit(): void {

    this.statusEntries = this.dataService.getStatusArray();
    this.dataService.getStatusFromServer();
  }

  ngOnDestroy(): void {}


  waitForSeconds(seconds: number): Promise<void> {
    return new Promise<void>((resolve) => {
      setTimeout(() => {
        resolve();
      }, seconds * 1000); // Convert seconds to milliseconds
    });
  }





}


