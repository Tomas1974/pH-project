import { Injectable } from '@angular/core';
import {AlertController, IonicSafeString} from "@ionic/angular";

@Injectable({
  providedIn: 'root'
})
export class UtilitiesService {

  constructor(private alertCtrl: AlertController) { }


  async confirmDelete(): Promise<boolean> {
    return new Promise<boolean>((resolve) => {
      this.alertCtrl.create({
        header: "Bekræft sletning",
        message: new IonicSafeString(`<img src="../assets/advarsel.png" style="max-width: 20%; height: auto"   />`),
        buttons: [
          {
            text: "Apply",
            handler: () => resolve(true)
          },
          {
            text: "Cancel",
            handler: () => resolve(false)
          }
        ]
      }).then(alert => {
        alert.present();
      });
    });
  }


}
