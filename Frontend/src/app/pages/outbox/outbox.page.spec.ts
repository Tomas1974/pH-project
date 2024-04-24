import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OutboxPage } from './outbox.page';

describe('OutboxPage', () => {
  let component: OutboxPage;
  let fixture: ComponentFixture<OutboxPage>;

  beforeEach(async(() => {
    fixture = TestBed.createComponent(OutboxPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
