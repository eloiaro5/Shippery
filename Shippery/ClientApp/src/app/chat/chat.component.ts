import { Component } from '@angular/core';

@Component({
  selector: 'chat-app',
  templateUrl: "./chat.component.html"
}) export class ChatComponent {
  public name = "Adrià";
  public lstMessages: string[] = ["buenos dias","que tal?","todo bien?"];
}
